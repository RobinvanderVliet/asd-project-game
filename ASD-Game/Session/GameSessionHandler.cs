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
using ASD_Game.Items.Services;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using ASD_Game.World.Models.Characters.StateMachine;
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
        private readonly IScreenHandler _screenHandler;
        private readonly IRelativeStatHandler _relativeStatHandler;
        private readonly IMessageService _messageService;
        private readonly IDatabaseService<PlayerItemPOCO> _playerItemDatabaseService;
        private readonly IGameConfigurationHandler _gameConfigurationHandler;
        private IDatabaseService<GameConfigurationPOCO> _gameConfigDatabaseService;
        private readonly IMoveHandler _moveHandler;
        private IItemService _itemService;
        private Timer AIUpdateTimer;
        private Random _random = new Random();
        private int MAX_HEALTH = 100;
        private int _brainUpdateTime = 10000;

        public GameSessionHandler(
            IClientController clientController, 
            IWorldService worldService,
            ISessionHandler sessionHandler, 
            IDatabaseService<GamePOCO> gamePocoService,
            IDatabaseService<PlayerPOCO> playerService, 
            IScreenHandler screenHandler,
            IRelativeStatHandler relativeStatHandler,
            IDatabaseService<PlayerItemPOCO> playerItemDatabaseService,
            IMessageService messageService,
            IItemService itemService, 
            IGameConfigurationHandler gameConfigurationHandler,
            IDatabaseService<GameConfigurationPOCO> gameConfigDatabaseService,
                IMoveHandler moveHandler)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.GameSession);
            _sessionHandler = sessionHandler;
            _gamePocoService = gamePocoService;
            _playerService = playerService;
            _screenHandler = screenHandler;
            _relativeStatHandler = relativeStatHandler;
            _messageService = messageService;
            _playerItemDatabaseService = playerItemDatabaseService;
            _gameConfigurationHandler = gameConfigurationHandler;
            _gameConfigDatabaseService = gameConfigDatabaseService;
            _playerItemDatabaseService = playerItemDatabaseService;
            _itemService = itemService;
            _moveHandler = moveHandler;
            _worldService = worldService;
            CheckAITimer();
            UpdateBrain();
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
            var allItems = _playerItemDatabaseService.GetAllAsync();
            allPlayerId.Wait();
            allItems.Wait();
            var playerLocations = allPlayerId.Result.Where(x => x.GameGUID == _clientController.SessionId);
            var playerItems = allItems.Result.Where(x => x.GameGUID == _clientController.SessionId);

            startGameDTO.SavedPlayers = playerLocations.ToList();
            startGameDTO.GameGuid = _clientController.SessionId;
            startGameDTO.SavedPlayerItems = playerItems.ToList();

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
                _screenHandler.TransitionTo(new GameScreen());
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
                    PlayerGUID = player.Id,
                    GameGUID = _clientController.SessionId,
                    GameGUIDAndPlayerGuid = _clientController.SessionId + player.Id,
                    XPosition = player.XPosition,
                    YPosition = player.YPosition,
                    Health = MAX_HEALTH
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
                NPCDifficultyCurrent = (int)_gameConfigurationHandler.GetCurrentMonsterDifficulty(),
                NPCDifficultyNew = (int)_gameConfigurationHandler.GetNewMonsterDifficulty(),
                ItemSpawnRate = (int)_gameConfigurationHandler.GetItemSpawnRate()
            };
            _gameConfigDatabaseService.CreateAsync(gameConfigurationPOCO);
        }

        private void AddItemsToPlayer(string playerId, string gameId)
        {
            PlayerItemPOCO poco = new()
            { PlayerGUID = playerId, ItemName = ItemFactory.GetBandana().ItemName, GameGUID = gameId };
            _ = _playerItemDatabaseService.CreateAsync(poco);

            poco = new() { PlayerGUID = playerId, ItemName = ItemFactory.GetKnife().ItemName, GameGUID = gameId };
            _ = _playerItemDatabaseService.CreateAsync(poco);
        }

        private void InsertGameIntoDatabase()
        {
            var gamePOCO = new GamePOCO
            {
                GameGUID = _clientController.SessionId,
                PlayerGUIDHost = _clientController.GetOriginId(),
                GameName = _sessionHandler.GameName,
                Seed = _sessionHandler.GetSessionSeed()
            };
            _gamePocoService.CreateAsync(gamePOCO);
        }

        private void HandleStartGameSession(StartGameDTO startGameDTO)
        {
            bool handleInDatabase = (_clientController.IsHost() || _clientController.IsBackupHost);


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

        private Player AddPlayerToWorldSavedGame(List<PlayerPOCO> savedPlayers)
        {
            Player currentPlayer = null;
            foreach (var player in savedPlayers)
            {
                if (_clientController.GetOriginId() == player.PlayerGUID)
                {
                    currentPlayer = new Player(player.PlayerName, player.XPosition, player.YPosition,
                        CharacterSymbol.CURRENT_PLAYER, player.PlayerGUID);
                    _worldService.AddPlayerToWorld(currentPlayer, true);
                    currentPlayer.Health = player.Health;
                    currentPlayer.Stamina = player.Stamina;
                    currentPlayer.RadiationLevel = player.RadiationLevel;
                }
                else
                {
                    var enemyPlayer = new Player(player.PlayerName, player.XPosition, player.YPosition,
                        CharacterSymbol.ENEMY_PLAYER, player.PlayerGUID);
                    _worldService.AddPlayerToWorld(enemyPlayer, false);
                    enemyPlayer.Health = player.Health;
                    enemyPlayer.Stamina = player.Stamina;
                    enemyPlayer.RadiationLevel = player.RadiationLevel;
                }
            }

            return currentPlayer;
        }
        private void CreateMonsters()
        {    for (int i = 0; i < 10; i++)
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
                        CharacterSymbol.TERMINATOR, "monst" + i);
                    SetBrain(newMonster);
                    _worldService.AddCreatureToWorld(newMonster);
                }
            }
        }
        
        private void SetBrain(SmartMonster monster)
        {    if (_sessionHandler.TrainingScenario != null)
            {
                if (_sessionHandler.TrainingScenario.BrainTransplant() != null)
                {
                    monster.Brain = _sessionHandler.TrainingScenario.BrainTransplant();
                }
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

        public void SetStateMachine(Monster monster)
        {
            ICharacterStateMachine CSM = new MonsterStateMachine(monster.MonsterData, null);
            monster.MonsterStateMachine = CSM;
        }
    }
}