using DatabaseHandler;
using System.Collections.Generic;
using System.Linq;
using ActionHandling;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Items;
using Messages;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
using Session.GameConfiguration;
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
        private readonly IDatabaseService<PlayerItemPOCO> _playerItemDatabaseService;
        private readonly IGameConfigurationHandler _gameConfigurationHandler;
        private IDatabaseService<GameConfigurationPOCO> _gameConfigDatabaseService;

        public GameSessionHandler(IClientController clientController, IWorldService worldService,
            ISessionHandler sessionHandler, IDatabaseService<GamePOCO> gamePocoService,
            IDatabaseService<PlayerPOCO> playerService,
            IDatabaseService<ClientHistoryPOCO> clientHistoryService, IScreenHandler screenHandler,
            IRelativeStatHandler relativeStatHandler, IMessageService messageService,
            IDatabaseService<PlayerItemPOCO> playerItemDatabaseService,
            IGameConfigurationHandler gameConfigurationHandler,
            IDatabaseService<GameConfigurationPOCO> gameConfigDatabaseService)
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
            _playerItemDatabaseService = playerItemDatabaseService;
            _gameConfigurationHandler = gameConfigurationHandler;
            _gameConfigDatabaseService = gameConfigDatabaseService;
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
                startGameDTO = new StartGameDTO();
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

        private Player AddPlayersToWorld()
        {
            List<string[]> allClients = _sessionHandler.GetAllClients();

            int playerX = 26;
            int playerY = 11;

            Player currentPlayer = null;
            foreach (var client in allClients)
            {
                if (_clientController.GetOriginId() == client[0])
                {
                    // add name to players
                    currentPlayer = new Player(client[1], playerX, playerY,
                        CharacterSymbol.CURRENT_PLAYER, client[0]);
                    _worldService.AddPlayerToWorld(currentPlayer, true);
                }
                else
                {
                    var playerObject = new Player(client[1], playerX, playerY, CharacterSymbol.ENEMY_PLAYER, client[0]);
                    _worldService.AddPlayerToWorld(playerObject, false);
                }
                playerX += 2;
                playerY += 2;
            }

            return currentPlayer;
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

        private void InsertPlayersIntoDatabase()
        {
            var players = _worldService.GetPlayers();
            foreach (Player player in players)
            {
                PlayerPOCO playerPoco = new PlayerPOCO
                {
                    PlayerGuid = player.Id, GameGuid = _clientController.SessionId,
                    GameGUIDAndPlayerGuid = _clientController.SessionId + player.Id, XPosition = player.XPosition,
                    YPosition = player.YPosition
                };
                _playerService.CreateAsync(playerPoco);
                AddItemsToPlayer(player.Id, _clientController.SessionId);
            }
        }

        private void InsertConfigurationIntoDatabase()
        {
            var gameConfigurationPOCO = new GameConfigurationPOCO
            {
                GameGUID = _clientController.SessionId,
                NPCDifficultyCurrent = (int) _gameConfigurationHandler.GetCurrentMonsterDifficulty(),
                NPCDifficultyNew = (int) _gameConfigurationHandler.GetNewMonsterDifficulty(),
                ItemSpawnRate = (int) _gameConfigurationHandler.GetSpawnRate()
            };
            _gameConfigDatabaseService.CreateAsync(gameConfigurationPOCO);
        }

        private void AddItemsToPlayer(string playerId, string gameId)
        {
            PlayerItemPOCO poco = new()
                {PlayerGUID = playerId, ItemName = ItemFactory.GetBandana().ItemName, GameGUID = gameId};
            _ = _playerItemDatabaseService.CreateAsync(poco);

            poco = new() {PlayerGUID = playerId, ItemName = ItemFactory.GetKnife().ItemName, GameGUID = gameId};
            _ = _playerItemDatabaseService.CreateAsync(poco);
        }

        private void InsertGameIntoDatabase()
        {
            var gamePOCO = new GamePOCO
                {GameGuid = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId()};
            _gamePocoService.CreateAsync(gamePOCO);
        }

        private void HandleStartGameSession(StartGameDTO startGameDTO)
        {
            bool handleInDatabase = (_clientController.IsHost() || _clientController.IsBackupHost);

            _screenHandler.TransitionTo(new GameScreen());
            Player currentPlayer = null;

            if (startGameDTO.GameGuid == null && !_sessionHandler.GameStarted())
            {
                _worldService.GenerateWorld(_sessionHandler.GetSessionSeed());
                currentPlayer = AddPlayersToWorld();

                if (handleInDatabase)
                {
                    InsertGameIntoDatabase();
                    InsertPlayersIntoDatabase();
                    InsertConfigurationIntoDatabase();
                }
            }
            else
            {
                _worldService.GenerateWorld(startGameDTO.Seed);
                currentPlayer = AddPlayerToWorldSavedGame(startGameDTO.SavedPlayers);
            }
            
            if (currentPlayer != null)
            {
                _worldService.LoadArea(currentPlayer.XPosition, currentPlayer.YPosition, 10);
            }
            
            _relativeStatHandler.SetCurrentPlayer(_worldService.GetCurrentPlayer());
            _relativeStatHandler.CheckStaminaTimer();
            _relativeStatHandler.CheckRadiationTimer();
            _worldService.DisplayWorld();
            _worldService.DisplayStats();
            _messageService.DisplayMessages();

        }

      
        private Player AddPlayersToNewGame(StartGameDTO startGameDTO)
        {
            Player currentPlayer = null;
            foreach (var player in startGameDTO.PlayerLocations)
            {
                if (_clientController.GetOriginId() == player.Key)
                {
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