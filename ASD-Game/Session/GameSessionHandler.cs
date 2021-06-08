using DatabaseHandler;
using System.Collections.Generic;
using System.Linq;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
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

        public GameSessionHandler(IClientController clientController, IWorldService worldService,
            ISessionHandler sessionHandler, IDatabaseService<GamePOCO> gamePocoService,
            IDatabaseService<PlayerPOCO> playerService, IDatabaseService<ClientHistoryPOCO> clientHistoryService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.GameSession);
            _worldService = worldService;
            _sessionHandler = sessionHandler;
            _gamePocoService = gamePocoService;
            _playerService = playerService;
            _clientHistoryService = clientHistoryService;
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
            StartGameDTO startGameDTO = new ();

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
                GameGuid = _clientController.SessionId,
                PlayerGUIDHost = _clientController.GetOriginId(),
                Seed = _sessionHandler.GetSessionSeed(),
                GameName = _sessionHandler.GameName
            };
            _gamePocoService.CreateAsync(gamePOCO);

            List<string> allClients = _sessionHandler.GetAllClients();
            Dictionary<string, int[]> players = new();

            players = SetupPositionsNewPlayers(allClients, gamePOCO);

            startGameDTO.GameGuid = gamePOCO.GameGuid;
            startGameDTO.PlayerLocations = players;

            return startGameDTO;
        }

        private Dictionary<string, int[]> SetupPositionsNewPlayers(List<string> allClients, GamePOCO gamePOCO)
        {
            Dictionary<string, int[]> players = new();
            int playerX = 26; // spawn position
            int playerY = 11; // spawn position

            foreach (var clientId in allClients)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = playerX;
                playerPosition[1] = playerY;
                players.Add(clientId, playerPosition);
                var tmpPlayer = new PlayerPOCO
                {
                    PlayerGuid = clientId,
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
            if (startGameDTO.ExistingPlayer != null && _clientController.GetOriginId() == startGameDTO.ExistingPlayer.PlayerGuid)
            {
                _worldService.GenerateWorld(startGameDTO.Seed);
                AddPlayersToNewGame(startGameDTO);
                _worldService.DisplayWorld();
            }

            if (_sessionHandler.GetSavedGame() && !_sessionHandler.GameStarted())
            {
                _worldService.GenerateWorld(startGameDTO.Seed);
                AddPlayerToWorldSavedGame(startGameDTO.SavedPlayers);
                _worldService.DisplayWorld();
            }
            
            else
            {
                CheckClientExists(startGameDTO);
            }
            _worldService.DisplayWorld();
        }

        private void CheckClientExists(StartGameDTO startGameDTO)
        {
            if (startGameDTO.ExistingPlayer != null)
            {
                if (_clientController.GetOriginId() == startGameDTO.ExistingPlayer.PlayerGuid)
                {
                    _worldService.GenerateWorld(startGameDTO.Seed);
                    _worldService.DisplayWorld();

                    AddPlayersToNewGame(startGameDTO);

                }
            }
            else
            {
                _worldService.GenerateWorld(_sessionHandler.GetSessionSeed());
                AddPlayersToNewGame(startGameDTO);
                _worldService.DisplayWorld();
            }
        }

        private void AddPlayersToNewGame(StartGameDTO startGameDTO)
        {
            _worldService.DisplayWorld();

            foreach (var player in startGameDTO.PlayerLocations)
            {
                if (_clientController.GetOriginId() == player.Key)
                {
                    var tmpObject = new ClientHistoryPOCO()
                    { PlayerId = player.Key, GameId = startGameDTO.GameGuid };
                    _clientHistoryService.CreateAsync(tmpObject);

                    if (startGameDTO.ExistingPlayer is null)
                    {
                        _worldService.AddPlayerToWorld(
                            new WorldGeneration.Player("gerrit", player.Value[0], player.Value[1],
                                CharacterSymbol.CURRENT_PLAYER, player.Key), true);
                    }
                    else
                    {
                        // Has to be merged with feature branche to set old health and stamina etc
                        _worldService.AddPlayerToWorld(
                            new WorldGeneration.Player("gerrit", startGameDTO.ExistingPlayer.XPosition, startGameDTO.ExistingPlayer.YPosition,
                                CharacterSymbol.CURRENT_PLAYER, player.Key), true);
                    }
                }
                else
                {
                    _worldService.AddPlayerToWorld(
                        new WorldGeneration.Player("arie", player.Value[0], player.Value[1],
                            CharacterSymbol.ENEMY_PLAYER, player.Key), false);
                }
            }

            _worldService.DisplayWorld();
        }

        private void AddPlayerToWorldSavedGame(List<PlayerPOCO> savedPlayers)
        {
            _worldService.DisplayWorld();

            foreach (var player in savedPlayers)
            {
                if (_clientController.GetOriginId() == player.PlayerGuid)
                {
                    _worldService.AddPlayerToWorld(
                        new WorldGeneration.Player("gerrit", player.XPosition, player.YPosition,
                            CharacterSymbol.CURRENT_PLAYER, player.PlayerGuid, player.Health, player.Stamina), true);
                }
                else
                {
                    _worldService.AddPlayerToWorld(
                        new WorldGeneration.Player("arie", player.XPosition, player.YPosition,
                            CharacterSymbol.ENEMY_PLAYER, player.PlayerGuid, player.Health, player.Stamina), false);
                }
            }

            _worldService.DisplayWorld();
        }
    }
}