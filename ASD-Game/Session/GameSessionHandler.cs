using DatabaseHandler;
using System.Collections.Generic;
using System.Linq;
using ActionHandling;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Messages;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
using UserInterface;
using WorldGeneration;
using WorldGeneration.Models;

namespace Session
{
    public class GameSessionHandler : IPacketHandler, IGameSessionHandler
    {
        private readonly IClientController _clientController;
        private readonly ISessionHandler _sessionHandler;
        private readonly IWorldService _worldService;
        private readonly IDatabaseService<PlayerPOCO> _playerService;
        private readonly IDatabaseService<GamePOCO> _gamePocoService;
        private readonly IDatabaseService<ClientHistoryPOCO> _clientHistoryService;
        private readonly IScreenHandler _screenHandler;
        private readonly IRelativeStatHandler _relativeStatHandler;
        private readonly IMessageService _messageService;

        public GameSessionHandler(IClientController clientController, IWorldService worldService,
            ISessionHandler sessionHandler, IDatabaseService<GamePOCO> gamePocoService,
            IDatabaseService<PlayerPOCO> playerService,
            IDatabaseService<ClientHistoryPOCO> clientHistoryService, IScreenHandler screenHandler,
            IRelativeStatHandler relativeStatHandler, IMessageService messageService
        )
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.GameSession);
            _worldService = worldService;
            _sessionHandler = sessionHandler;
            _gamePocoService = gamePocoService;
            _playerService = playerService;
            _clientHistoryService = clientHistoryService;
            _screenHandler = screenHandler;
            _relativeStatHandler = relativeStatHandler;
            _messageService = messageService;
        }

        public void SendGameSession()
        {
            StartGameDTO startGameDTO;
            if (_sessionHandler.GetSavedGame())
            {
                startGameDTO = LoadSave();
            }
            else
            {
                startGameDTO = SetupGameHost();
            }

            SendGameSessionDTO(startGameDTO);
            _sessionHandler.SetGameStarted(true);
        }

        private StartGameDTO LoadSave()
        {
            StartGameDTO startGameDTO = new();

            var allPlayers = _playerService.GetAllAsync();
            allPlayers.Wait();
            var allPlayersInGame = allPlayers.Result.Where(x => x.GameGuid == _clientController.SessionId);

            startGameDTO = SetLoadedGameInfo(startGameDTO, allPlayersInGame);

            return startGameDTO;
        }

        private StartGameDTO SetLoadedGameInfo(StartGameDTO startGameDTO, IEnumerable<PlayerPOCO> allPlayersInGame)
        {
            startGameDTO.SavedPlayers = new List<PlayerPOCO>();
            Dictionary<string, int[]> players = new Dictionary<string, int[]>();
            _sessionHandler.GetSessionSeed();

            foreach (var player in allPlayersInGame)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = player.XPosition;
                playerPosition[1] = player.YPosition;

                players.Add(player.PlayerGuid, playerPosition);
                startGameDTO.SavedPlayers.Add(player);
            }

            startGameDTO.PlayerLocations = players;
            return startGameDTO;
        }

        public StartGameDTO SetupGameHost()
        {
            StartGameDTO startGameDTO = new StartGameDTO();

            var gamePOCO = new GamePOCO
            {
                GameGuid = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId(),
                GameName = _sessionHandler.GameName, Seed = _sessionHandler.GetSessionSeed()
            };
            _gamePocoService.CreateAsync(gamePOCO);


            List<string[]> allClients = _sessionHandler.GetAllClients();
            Dictionary<string, int[]> players = new();

            players = SetupPositionsNewPlayers(allClients, gamePOCO);

            startGameDTO.GameGuid = gamePOCO.GameGuid;
            startGameDTO.PlayerLocations = players;

            return startGameDTO;
        }

        private Dictionary<string, int[]> SetupPositionsNewPlayers(List<string[]> allClients, GamePOCO gamePOCO)
        {
            Dictionary<string, int[]> players = new();
            int playerX = 26; // spawn position
            int playerY = 11; // spawn position

            foreach (var clientId in allClients)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = playerX;
                playerPosition[1] = playerY;
                players.Add(clientId[0], playerPosition);
                var tmpPlayer = new PlayerPOCO
                {
                    PlayerGuid = clientId[0][0].ToString(),
                    GameGuid = gamePOCO.GameGuid,
                    XPosition = playerX,
                    YPosition = playerY,
                    GameGUIDAndPlayerGuid = gamePOCO.GameGuid + clientId
                };

                var insert = _playerService.CreateAsync(tmpPlayer);
                insert.Wait();

                playerX += 2; // spawn position + 2 each client
                playerY += 2; // spawn position + 2 each client
            }

            return players;
        }

        private void SendGameSessionDTO(StartGameDTO startGameDTO)
        {
            var payload = JsonConvert.SerializeObject(startGameDTO);
            _clientController.SendPayload(payload, PacketType.GameSession);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var startGameDTO = JsonConvert.DeserializeObject<StartGameDTO>(packet.Payload);
            HandleStartGameSession(startGameDTO);
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void HandleStartGameSession(StartGameDTO startGameDTO)
        {
            _screenHandler.TransitionTo(new GameScreen());
            Player currentPlayer = null;

            if (startGameDTO.ExistingPlayer != null &&
                _clientController.GetOriginId() == startGameDTO.ExistingPlayer.PlayerGuid)
            {
                _worldService.GenerateWorld(startGameDTO.Seed);
                currentPlayer = AddPlayersToNewGame(startGameDTO);
            }

            if (_sessionHandler.GetSavedGame() && !_sessionHandler.GameStarted())
            {
                _worldService.GenerateWorld(startGameDTO.Seed);
                currentPlayer = AddPlayerToWorldSavedGame(startGameDTO.SavedPlayers);
            }

            else
            {
                currentPlayer = CheckClientExists(startGameDTO);
            }

            _relativeStatHandler.SetCurrentPlayer(_worldService.GetCurrentPlayer());
            _relativeStatHandler.CheckStaminaTimer();
            _relativeStatHandler.CheckRadiationTimer();
            _worldService.DisplayWorld();
            _worldService.DisplayStats();
            _messageService.DisplayMessages();

            if (currentPlayer != null)
            {
                _worldService.LoadArea(currentPlayer.XPosition, currentPlayer.YPosition, 10);
            }
        }

        private Player CheckClientExists(StartGameDTO startGameDTO)
        {
            if (startGameDTO.ExistingPlayer != null)
            {
                if (_clientController.GetOriginId() == startGameDTO.ExistingPlayer.PlayerGuid)
                {
                    _worldService.GenerateWorld(startGameDTO.Seed);
                    return AddPlayersToNewGame(startGameDTO);
                }
            }
            else
            {
                _worldService.GenerateWorld(_sessionHandler.GetSessionSeed());
                return AddPlayersToNewGame(startGameDTO);
            }

            return null;
        }

        private Player AddPlayersToNewGame(StartGameDTO startGameDTO)
        {
            Player currentPlayer = null;
            foreach (var player in startGameDTO.PlayerLocations)
            {
                if (_clientController.GetOriginId() == player.Key)
                {
                    var tmpObject = new ClientHistoryPOCO()
                        {PlayerId = player.Key, GameId = startGameDTO.GameGuid};
                    _clientHistoryService.CreateAsync(tmpObject);

                    if (startGameDTO.ExistingPlayer is null)
                    {
                        currentPlayer = new Player("gerrit", player.Value[0], player.Value[1],
                            CharacterSymbol.CURRENT_PLAYER, player.Key);
                        _worldService.AddPlayerToWorld(currentPlayer, true);
                    }
                    else
                    {
                        currentPlayer = new Player("arie", startGameDTO.ExistingPlayer.XPosition,
                            startGameDTO.ExistingPlayer.YPosition,
                            CharacterSymbol.CURRENT_PLAYER, player.Key);
                        _worldService.AddPlayerToWorld(currentPlayer, true);
                    }
                }
                else
                {
                    var playerObject = new Player("barry", player.Value[0], player.Value[0],
                        CharacterSymbol.ENEMY_PLAYER, player.Key);
                    _worldService.AddPlayerToWorld(playerObject, false);
                }
            }

            return currentPlayer;
        }

        private Player AddPlayerToWorldSavedGame(List<PlayerPOCO> savedPlayers)
        {
            Player currentPlayer = null;
            foreach (var player in savedPlayers)
            {
                if (_clientController.GetOriginId() == player.PlayerGuid)
                {
                    currentPlayer = new Player("gerrit", player.XPosition, player.YPosition,
                        CharacterSymbol.CURRENT_PLAYER, player.PlayerGuid);
                    _worldService.AddPlayerToWorld(currentPlayer, true);
                }
                else
                {
                    var enemyPlayer = new Player("arie", player.XPosition, player.YPosition,
                        CharacterSymbol.ENEMY_PLAYER, player.PlayerGuid);
                    _worldService.AddPlayerToWorld(enemyPlayer, false);
                }
            }

            return currentPlayer;
        }
    }
}