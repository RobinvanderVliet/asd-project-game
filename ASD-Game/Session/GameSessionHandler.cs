using Creature.Creature;
using Creature.Creature.NeuralNetworking.TrainingScenario;
using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using Items;
using Messages;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
using Session.GameConfiguration;
using System;
using System.Collections.Generic;
using System.Timers;
using UserInterface;
using WorldGeneration;
using WorldGeneration.Models;
using WorldGeneration.StateMachine;

namespace Session
{
    public class GameSessionHandler : IPacketHandler, IGameSessionHandler
    {
        private IClientController _clientController;
        private ISessionHandler _sessionHandler;
        private IWorldService _worldService;
        private IMessageService _messageService;
        private IGameConfigurationHandler _gameConfigurationHandler;
        private IScreenHandler _screenHandler;

        private IDatabaseService<PlayerPOCO> _playerServicesDb;
        private IDatabaseService<GamePOCO> _gameServicesDb;
        private IDatabaseService<GameConfigurationPOCO> _gameConfigServicesDb;
        private IDatabaseService<PlayerItemPoco> _playerItemServicesDb;

        private Random random = new Random();
        private Timer AIUpdateTimer;

        public GameSessionHandler(IClientController clientController, IWorldService worldService, ISessionHandler sessionHandler, IDatabaseService<PlayerPOCO> playerServicesDb,
            IDatabaseService<GamePOCO> gameServicesDb, IDatabaseService<GameConfigurationPOCO> gameConfigservicesDb, IGameConfigurationHandler gameConfigurationHandler,
            IScreenHandler screenHandler, IDatabaseService<PlayerItemPoco> playerItemServicesDb, IMessageService messageService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.GameSession);
            _worldService = worldService;
            _messageService = messageService;
            _sessionHandler = sessionHandler;
            _gameConfigurationHandler = gameConfigurationHandler;
            _playerServicesDb = playerServicesDb;
            _gameServicesDb = gameServicesDb;
            _gameConfigServicesDb = gameConfigservicesDb;
            _screenHandler = screenHandler;
            _playerItemServicesDb = playerItemServicesDb;
            AIUpdateTimer = new Timer(60000);
            CheckAITimer();
        }

        public void SendGameSession()
        {
            var StartGameDTO = SetupGameHost();
            SendGameSessionDTO(StartGameDTO);
        }

        public StartGameDTO SetupGameHost()
        {
            var gameConfigurationPOCO = new GameConfigurationPOCO
            {
                GameGUID = _clientController.SessionId,
                NPCDifficultyCurrent = (int)_gameConfigurationHandler.GetCurrentMonsterDifficulty(),
                NPCDifficultyNew = (int)_gameConfigurationHandler.GetNewMonsterDifficulty(),
                ItemSpawnRate = (int)_gameConfigurationHandler.GetSpawnRate()
            };
            _gameConfigServicesDb.CreateAsync(gameConfigurationPOCO);

            var gamePOCO = new GamePOCO { GameGUID = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId() };

            _gameServicesDb.CreateAsync(gamePOCO);

            List<string[]> allClients = _sessionHandler.GetAllClients();

            Dictionary<string, int[]> players = new Dictionary<string, int[]>();

            // Needs to be refactored to something random in construction; this was for testing
            int playerX = 26; // spawn position
            int playerY = 11; // spawn position
            foreach (string[] client in allClients)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = playerX;
                playerPosition[1] = playerY;
                players.Add(client[0], playerPosition);
                var tmpPlayer = new PlayerPOCO
                { PlayerGuid = client[0], GameGuid = gamePOCO.GameGUID, GameGUIDAndPlayerGuid = gamePOCO.GameGUID + client[0], XPosition = playerX, YPosition = playerY };
                _playerServicesDb.CreateAsync(tmpPlayer);
                AddItemsToPlayer(client[0], gamePOCO.GameGUID);

                playerX += 2; // spawn position + 2 each client
                playerY += 2; // spawn position + 2 each client
            }

            StartGameDTO startGameDTO = new StartGameDTO();
            startGameDTO.GameGuid = _clientController.SessionId;
            startGameDTO.PlayerLocations = players;

            return startGameDTO;
        }

        private void AddItemsToPlayer(string playerId, string gameId)
        {
            PlayerItemPoco poco = new() { PlayerGUID = playerId, ItemName = ItemFactory.GetBandana().ItemName, GameGUID = gameId };
            _ = _playerItemServicesDb.CreateAsync(poco);

            poco = new() { PlayerGUID = playerId, ItemName = ItemFactory.GetKnife().ItemName, GameGUID = gameId };
            _ = _playerItemServicesDb.CreateAsync(poco);
        }

        private void SendGameSessionDTO(StartGameDTO startGameDTO)
        {
            var payload = JsonConvert.SerializeObject(startGameDTO);
            _clientController.SendPayload(payload, PacketType.GameSession);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            _screenHandler.TransitionTo(new GameScreen());
            var startGameDTO = JsonConvert.DeserializeObject<StartGameDTO>(packet.Payload);
            HandleStartGameSession(startGameDTO);
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void HandleStartGameSession(StartGameDTO startGameDTO)
        {
            _worldService.GenerateWorld(_sessionHandler.GetSessionSeed());

            // add name to player
            foreach (var player in startGameDTO.PlayerLocations)
            {
                if (_clientController.GetOriginId() == player.Key)
                {
                    // add name to players
                    var playerObject = new Player("gerrit", player.Value[0], player.Value[1], CharacterSymbol.CURRENT_PLAYER, player.Key);
                    _worldService.AddPlayerToWorld(playerObject, true);
                }
                else
                {
                    var playerObject = new Player("arie", player.Value[0], player.Value[1], CharacterSymbol.ENEMY_PLAYER, player.Key);
                    _worldService.AddPlayerToWorld(playerObject, false);
                }
            }
            CreateMonsters();
            _worldService.DisplayWorld();
        }

        private void CreateMonsters()
        {
            for (int i = 0; i < 10; i++)
            {
                if (i < 5)
                {
                    Monster newMonster = new Monster("Zombie", random.Next(12, 25), random.Next(12, 25), CharacterSymbol.TERMINATOR, "monst" + i);
                    setStateMachine(newMonster);
                    _worldService.AddCreatureToWorld(newMonster);
                }
                else
                {
                    SmartMonster newMonster = new SmartMonster("Zombie", random.Next(12, 25), random.Next(12, 25), CharacterSymbol.TERMINATOR, "monst" + i, new DataGatheringService(_worldService));
                    setBrain(newMonster);
                    _worldService.AddCreatureToWorld(newMonster);
                }
            }
        }

        private void setBrain(SmartMonster monster)
        {
            if (_sessionHandler.trainingScenario.brainTransplant() != null)
            {
                monster.brain = _sessionHandler.trainingScenario.brainTransplant();
            }
        }

        private void CheckAITimer()
        {
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
            _worldService.UpdateBrains(_sessionHandler.trainingScenario.brainTransplant());
        }

        private void setStateMachine(Monster monster)
        {
            ICharacterStateMachine CSM = new MonsterStateMachine(monster.MonsterData, null);
            monster.MonsterStateMachine = CSM;
        }
    }
}