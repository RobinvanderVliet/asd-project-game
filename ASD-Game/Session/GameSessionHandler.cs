using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using Items;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
using Session.GameConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using ActionHandling;
using UserInterface;
using WorldGeneration;
using WorldGeneration.Models;
using Messages;

namespace Session
{
    public class GameSessionHandler : IPacketHandler, IGameSessionHandler
    {
        private readonly IClientController _clientController;
        private readonly ISessionHandler _sessionHandler;
        private readonly IRelativeStatHandler _relativeStatHandler;
        private readonly IGameConfigurationHandler _gameConfigurationHandler;
        private readonly IScreenHandler _screenHandler;
        private readonly IDatabaseService<PlayerPOCO> _playerDatabaseService;
        private readonly IDatabaseService<GamePOCO> _gameDatabaseService;
        private readonly IDatabaseService<GameConfigurationPOCO> _gameConfigDatabaseService;
        private readonly IDatabaseService<PlayerItemPOCO> _playerItemDatabaseService;
        private readonly IWorldService _worldService;
        private readonly IMessageService _messageService;

        public GameSessionHandler(
            IClientController clientController,
            ISessionHandler sessionHandler,
            IRelativeStatHandler relativeStatHandler,
            IGameConfigurationHandler gameConfigurationHandler,
            IScreenHandler screenHandler,
            IDatabaseService<PlayerPOCO> playerDatabaseService,
            IDatabaseService<GamePOCO> gameDatabaseService,
            IDatabaseService<GameConfigurationPOCO> gameConfigDatabaseService,
            IDatabaseService<PlayerItemPOCO> playerItemDatabaseService,
            IWorldService worldService,
            IMessageService messageService
        )
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.GameSession);
            _sessionHandler = sessionHandler;
            _relativeStatHandler = relativeStatHandler;
            _gameConfigurationHandler = gameConfigurationHandler;
            _screenHandler = screenHandler;
            _playerDatabaseService = playerDatabaseService;
            _gameDatabaseService = gameDatabaseService;
            _gameConfigDatabaseService = gameConfigDatabaseService;
            _playerItemDatabaseService = playerItemDatabaseService;
            _worldService = worldService;
            _messageService = messageService;
        }

        public void SendGameSession()
        {
            //If startGameDTO is null then it will create new database in handlepacket, else it will get old saveddata. 
            StartGameDTO startGameDTO = new StartGameDTO();
            if (_sessionHandler.GetSavedGame())
            {
                startGameDTO = LoadSave();
            }

            SendGameSessionDTO(startGameDTO);
            _sessionHandler.SetGameStarted(true);
        }

        private void AddItemsToPlayer(string playerId, string gameId)
        {
            PlayerItemPOCO poco = new()
                {PlayerGUID = playerId, ItemName = ItemFactory.GetBandana().ItemName, GameGUID = gameId};
            _ = _playerItemDatabaseService.CreateAsync(poco);

            poco = new() {PlayerGUID = playerId, ItemName = ItemFactory.GetKnife().ItemName, GameGUID = gameId};
            _ = _playerItemDatabaseService.CreateAsync(poco);
        }

        private void SendGameSessionDTO(StartGameDTO startGameDTO)
        {
            var payload = JsonConvert.SerializeObject(startGameDTO);
            _clientController.SendPayload(payload, PacketType.GameSession);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            bool handleInDatabase = (_clientController.IsHost() && packet.Header.Target.Equals("host")) ||
                                    _clientController.IsBackupHost;
            var startGameDTO = JsonConvert.DeserializeObject<StartGameDTO>(packet.Payload);
            
            
            if (startGameDTO.ExistingPlayer == null && !_sessionHandler.GameStarted())
            {
                _screenHandler.TransitionTo(new GameScreen());
                _worldService.GenerateWorld(_sessionHandler.GetSessionSeed());
            }

            Player currentPlayer;

            if (startGameDTO.ExistingPlayer != null &&
                _clientController.GetOriginId() == startGameDTO.ExistingPlayer.PlayerGuid && !_sessionHandler.GameStarted() )
            {
                _screenHandler.TransitionTo(new GameScreen());
                _worldService.GenerateWorld(startGameDTO.Seed);
                currentPlayer = AddOldPlayersToNewGame(startGameDTO);
            }
            else if (_sessionHandler.GetSavedGame() && !_sessionHandler.GameStarted())
            {
                _worldService.GenerateWorld(_sessionHandler.GetSessionSeed());
                currentPlayer = AddPlayerToWorldSavedGame(startGameDTO.SavedPlayers);
            }
            else
            {
                currentPlayer = AddPlayersToWorld();
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

            if (handleInDatabase && !_sessionHandler.GameStarted())
            {
                InsertConfigurationIntoDatabase();
                if (startGameDTO.SavedPlayers == null)
                {
                    InsertGameIntoDatabase();

                    InsertPlayersIntoDatabase();
                }
            }

            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }
        
        private Player AddOldPlayersToNewGame(StartGameDTO startGameDTO)
        {
            Player currentPlayer = null;
            foreach (var player in startGameDTO.PlayerLocations)
            {
                if (_clientController.GetOriginId() == player.Key)
                {
                    currentPlayer = new Player("arie", player.Value[0], player.Value[1],
                        CharacterSymbol.ENEMY_PLAYER, player.Key);
                    _worldService.AddPlayerToWorld(currentPlayer, true);
                }
                else
                {
                    var playerObect = new Player("gerrit", startGameDTO.ExistingPlayer.XPosition,
                        startGameDTO.ExistingPlayer.YPosition,
                        CharacterSymbol.CURRENT_PLAYER, player.Key);
                    _worldService.AddPlayerToWorld(playerObect, false);
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
                    currentPlayer = new Player(player.PlayerName, player.XPosition, player.YPosition,
                        CharacterSymbol.CURRENT_PLAYER, player.PlayerGuid);
                    _worldService.AddPlayerToWorld(currentPlayer, true);
                }
                else
                {
                    var playerObject = new Player(player.PlayerName, player.XPosition, player.YPosition,
                        CharacterSymbol.ENEMY_PLAYER, player.PlayerGuid);
                    _worldService.AddPlayerToWorld(playerObject, false);
                }
            }

            return currentPlayer;
        }

        private Player AddRejoinedPlayerToGame(StartGameDTO startGameDTO)
        {
            Player currentPlayer = null;

            foreach (var player in startGameDTO.PlayerLocations)
            {
                if (_clientController.GetOriginId() == player.Key)
                {
                    var tmpClientHistory = new DatabaseService<ClientHistoryPOCO>();
                    var tmpObject = new ClientHistoryPOCO()
                        {PlayerId = player.Key, GameId = startGameDTO.GameGuid};
                    tmpClientHistory.CreateAsync(tmpObject);

                    if (startGameDTO.ExistingPlayer is null)
                    {
                        currentPlayer = new Player("gerrit", player.Value[0], player.Value[1],
                            CharacterSymbol.CURRENT_PLAYER, player.Key);
                        _worldService.AddPlayerToWorld(currentPlayer, true);
                    }
                    else
                    {
                        var playerObect = new Player("gerrit", startGameDTO.ExistingPlayer.XPosition,
                            startGameDTO.ExistingPlayer.YPosition,
                            CharacterSymbol.CURRENT_PLAYER, player.Key);
                        // Has to be merged with feature branche to set old health and stamina etc
                        _worldService.AddPlayerToWorld(playerObect, false);
                    }
                }
                else
                {
                    _worldService.AddPlayerToWorld(
                        new WorldGeneration.Player("arie", player.Value[0], player.Value[1],
                            CharacterSymbol.ENEMY_PLAYER, player.Key), false);
                }
            }

            return currentPlayer;
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
                _playerDatabaseService.CreateAsync(playerPoco);
                AddItemsToPlayer(player.Id, _clientController.SessionId);
            }
        }


        private void InsertGameIntoDatabase()
        {
            var gamePOCO = new GamePOCO
            {
                GameGuid = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId(),
                GameName = _sessionHandler.GameName, Seed = _sessionHandler.GetSessionSeed()
            };
            _gameDatabaseService.CreateAsync(gamePOCO);
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
                    PlayerGuid = clientId[0],
                    GameGuid = gamePOCO.GameGuid,
                    XPosition = playerX,
                    YPosition = playerY,
                    GameGUIDAndPlayerGuid = gamePOCO.GameGuid + clientId
                };

                var insert = _playerDatabaseService.CreateAsync(tmpPlayer);
                insert.Wait();

                playerX += 2; // spawn position + 2 each client
                playerY += 2; // spawn position + 2 each client
            }

            return players;
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

        private StartGameDTO LoadSave()
        {
            StartGameDTO startGameDTO = new();

            var allPlayers = _playerDatabaseService.GetAllAsync();
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
    }
}