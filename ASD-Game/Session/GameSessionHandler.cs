using System;
using System.Collections.Generic;
using ActionHandling;
using Agent.Services;
using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using Items;
using Messages;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
using Session.GameConfiguration;
using System.Timers;
using UserInterface;
using WorldGeneration;
using WorldGeneration.Models;
using WorldGeneration.StateMachine;
using World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario;
using World.Models.Characters.StateMachine.Data;
using System.Numerics;
using World.Models.Characters;

namespace Session
{
    public class GameSessionHandler : IPacketHandler, IGameSessionHandler
    {
        private readonly INetworkComponent _networkComponent;
        private readonly IConfigurationService _configurationService;
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
        private readonly IMoveHandler _moveHandler;
        private Timer AIUpdateTimer;
        private int _brainUpdateTime = 60000;
        private Random _random = new Random();

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
            IMessageService messageService,
            INetworkComponent networkComponent,
            IConfigurationService configurationService,
            IMoveHandler moveHandler
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
            _networkComponent = networkComponent;
            _configurationService = configurationService;
            _moveHandler = moveHandler;
            CheckAITimer();
        }

        // TODO: get this config from the AgentConfigurationService
        public void SendAgentConfiguration()
        { 
            _configurationService.CreateConfiguration("agent");
            var configuration = _configurationService.Configuration;
            var agentConfigurationDto = new AgentConfigurationDTO(SessionType.SendAgentConfiguration)
            {
                PlayerId = _clientController.GetOriginId(),
                AgentConfiguration = configuration.Settings,
                GameGUID = _clientController.SessionId
            };
            var payload = JsonConvert.SerializeObject(agentConfigurationDto);
            _clientController.SendPayload(payload, PacketType.Agent);
        }
        
        public void SendGameSession()
        {
            StartGameDTO startGameDTO = new StartGameDTO();
            SendGameSessionDTO(startGameDTO);
        }

        private void AddItemsToPlayer(string playerId, string gameId)
        {
            PlayerItemPOCO poco = new() { PlayerGUID = playerId, ItemName = ItemFactory.GetBandana().ItemName, GameGUID = gameId };
            _ = _playerItemDatabaseService.CreateAsync(poco);

            poco = new() { PlayerGUID = playerId, ItemName = ItemFactory.GetKnife().ItemName, GameGUID = gameId };
            _ = _playerItemDatabaseService.CreateAsync(poco);
        }

        private void SendGameSessionDTO(StartGameDTO startGameDTO)
        {
            var payload = JsonConvert.SerializeObject(startGameDTO);
            _clientController.SendPayload(payload, PacketType.GameSession);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            bool handleInDatabase = (_clientController.IsHost() && packet.Header.Target.Equals("host")) || _clientController.IsBackupHost;

            _screenHandler.TransitionTo(new GameScreen());

            _worldService.GenerateWorld(_sessionHandler.GetSessionSeed());
            CreateMonsters();

            Player currentPlayer = AddPlayersToWorld();

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

            if (handleInDatabase)
            {
                InsertConfigurationIntoDatabase();
                InsertGameIntoDatabase();
                InsertPlayersIntoDatabase();
            }

            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void InsertPlayersIntoDatabase()
        {
            var players = _worldService.GetAllPlayers();
            foreach (Player player in players)
            {
                PlayerPOCO playerPoco = new PlayerPOCO { PlayerGuid = player.Id, GameGuid = _clientController.SessionId, GameGUIDAndPlayerGuid = _clientController.SessionId + player.Id, XPosition = player.XPosition, YPosition = player.YPosition };
                _playerDatabaseService.CreateAsync(playerPoco);
                AddItemsToPlayer(player.Id, _clientController.SessionId);
            }
        }

        private void InsertGameIntoDatabase()
        {
            var gamePOCO = new GamePOCO { GameGUID = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId() };
            _gameDatabaseService.CreateAsync(gamePOCO);
        }

        private void InsertConfigurationIntoDatabase()
        {
            SendAgentConfiguration();

            var gameConfigurationPOCO = new GameConfigurationPOCO
            {
                GameGUID = _clientController.SessionId,
                NPCDifficultyCurrent = (int)_gameConfigurationHandler.GetCurrentMonsterDifficulty(),
                NPCDifficultyNew = (int)_gameConfigurationHandler.GetNewMonsterDifficulty(),
                ItemSpawnRate = (int)_gameConfigurationHandler.GetSpawnRate()
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

        private void CreateMonsters()
        {
            for (int i = 0; i < 20; i++)
            {
                if (i >= 0)
                {
                    Monster newMonster = new Monster("Zombie", _random.Next(12, 25), _random.Next(12, 25), CharacterSymbol.TERMINATOR, "monst" + i);
                    MonsterData newMonsterData = new(newMonster.XPosition, newMonster.YPosition, 0);
                    newMonsterData.WorldService = _worldService;
                    newMonsterData.MoveHandler = _moveHandler;
                    newMonsterData.Position = new Vector2(newMonster.XPosition, newMonster.YPosition);
                    newMonsterData.CharacterId = newMonster.Id;
                    newMonster.MonsterData = newMonsterData;
                    SetStateMachine(newMonster);
                    newMonster.MonsterStateMachine.StartStateMachine();
                    _worldService.AddCreatureToWorld(newMonster);
                }
                    else
                {
                    SmartMonster newMonster = new SmartMonster("Zombie", _random.Next(12, 25), _random.Next(12, 25), CharacterSymbol.TERMINATOR, "monst" + i, new DataGatheringService(_worldService));
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
            ICharacterStateMachine CSM = new MonsterStateMachine(monster.MonsterData);
            monster.MonsterStateMachine = CSM;
        }
    }
}