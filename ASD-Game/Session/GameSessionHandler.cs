using ASD_Game.ActionHandling;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Items;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.Session.DTO;
using ASD_Game.Session.GameConfiguration;
using ASD_Game.Session.Helpers;
using ASD_Game.UserInterface;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using ASD_Game.World.Models.Characters.StateMachine;
using DatabaseHandler.POCO;
using WorldGeneration;
using WorldGeneration.StateMachine;

namespace ASD_Game.Session
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
        private Timer AIUpdateTimer;
        private int _brainUpdateTime = 60000;
        private Random _random = new Random();

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
            _playerItemDatabaseService = playerItemDatabaseService;
            _worldService = worldService;
            _messageService = messageService;
            CheckAITimer();
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

        public StartGameDTO LoadSave()
        {
            StartGameDTO startGameDTO = new StartGameDTO();
            startGameDTO.Seed = _sessionHandler.GetSessionSeed();

            var allPlayerId = _playerService.GetAllAsync();
            allPlayerId.Wait();
            var playerLocations = allPlayerId.Result.Where(x => x.GameGUID == _clientController.SessionId);

            startGameDTO.SavedPlayers = playerLocations.ToList();
            startGameDTO.GameGuid = _clientController.SessionId;


            return startGameDTO;
        }

        private Player AddPlayersToWorld()
        {
            return PlayerSpawner.SpawnPlayers(_sessionHandler.GetAllClients(), _sessionHandler.GetSessionSeed(), _worldService, _clientController);
        }


        private void SendGameSessionDTO(StartGameDTO startGameDTO)
        {
            var payload = JsonConvert.SerializeObject(startGameDTO);
            _clientController.SendPayload(payload, PacketType.GameSession);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var startGameDTO = JsonConvert.DeserializeObject<StartGameDTO>(packet.Payload);

            if (startGameDTO is not null)
            {
                HandleStartGameSession(startGameDTO);
                return new HandlerResponseDTO(SendAction.SendToClients, null);
            }

            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        private void InsertPlayersIntoDatabase()
        {
            var players = _worldService.GetAllPlayers();
            foreach (Player player in players)
            {
                PlayerPOCO playerPoco = new PlayerPOCO
                {
                    PlayerGUID = player.Id, GameGUID = _clientController.SessionId,
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
            {
                GameGUID = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId(),
                GameName = _sessionHandler.GameName, Seed = _sessionHandler.GetSessionSeed()
            };
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
                CreateMonsters();
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
                if (_clientController.GetOriginId() == player.PlayerGUID)
                {
                    currentPlayer = new Player("gerrit", player.XPosition, player.YPosition,
                        CharacterSymbol.CURRENT_PLAYER, player.PlayerGUID);
                    _worldService.AddPlayerToWorld(currentPlayer, true);
                }
                else
                {
                    var enemyPlayer = new Player("arie", player.XPosition, player.YPosition,
                        CharacterSymbol.ENEMY_PLAYER, player.PlayerGUID);
                    _worldService.AddPlayerToWorld(enemyPlayer, false);
                }
            }

            return currentPlayer;
        }

        private void CreateMonsters()
        {
            for (int i = 0; i < 10; i++)
            {
                if (i < 0)
                {
                    Monster newMonster = new Monster("Zombie", _random.Next(12, 25), _random.Next(12, 25),
                        CharacterSymbol.TERMINATOR, "monst" + i);
                    SetStateMachine(newMonster);
                    _worldService.AddCreatureToWorld(newMonster);
                }
                else
                {
                    SmartMonster newMonster = new SmartMonster("Zombie", _random.Next(12, 25), _random.Next(12, 25),
                        CharacterSymbol.TERMINATOR, "monst" + i, new DataGatheringService(_worldService));
                    SetBrain(newMonster);
                    _worldService.AddCreatureToWorld(newMonster);
                }
            }
        }

        private void SetBrain(SmartMonster monster)
        {
            if (_sessionHandler.TrainingScenario.BrainTransplant() != null)
            {
                monster.Brain = _sessionHandler.TrainingScenario.BrainTransplant();
            }
        }

        private void CheckAITimer()
        {
            AIUpdateTimer = new Timer(_brainUpdateTime);
            AIUpdateTimer.AutoReset = true;
            AIUpdateTimer.Elapsed += CheckAITimerEvent;
            AIUpdateTimer.Start();
        }

        private void CheckAITimerEvent(object sender, ElapsedEventArgs e)
        {
            AIUpdateTimer.Stop();
            UpdateBrain();
            AIUpdateTimer.Start();
        }

        public void UpdateBrain()
        {
            if (_sessionHandler.TrainingScenario.BrainTransplant() != null)
            {
                _worldService.UpdateBrains(_sessionHandler.TrainingScenario.BrainTransplant());
            }
        }

        private void SetStateMachine(Monster monster)
        {
            ICharacterStateMachine CSM = new MonsterStateMachine(monster.MonsterData, null);
            monster.MonsterStateMachine = CSM;
        }
    }
}