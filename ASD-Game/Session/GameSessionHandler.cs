using System.Collections.Generic;
using ASD_project.DatabaseHandler.POCO;
using ASD_project.DatabaseHandler.Services;
using ASD_project.Items;
using ASD_project.Network;
using ASD_project.Network.DTO;
using ASD_project.Network.Enum;
using ASD_project.Session.DTO;
using ASD_project.UserInterface;
using ASD_project.World.Models;
using ASD_project.World.Models.Characters;
using ASD_project.World.Services;
using DatabaseHandler.POCO;
using Messages;
using Newtonsoft.Json;
using Session.GameConfiguration;

namespace ASD_project.Session
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
        private IDatabaseService<PlayerItemPOCO> _playerItemServicesDb;

        public GameSessionHandler(IClientController clientController, IWorldService worldService, ISessionHandler sessionHandler, IDatabaseService<PlayerPOCO> playerServicesDb,
            IDatabaseService<GamePOCO> gameServicesDb, IDatabaseService<GameConfigurationPOCO> gameConfigservicesDb, IGameConfigurationHandler gameConfigurationHandler,
            IScreenHandler screenHandler, IDatabaseService<PlayerItemPOCO> playerItemServicesDb, IMessageService messageService)
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
                NPCDifficultyCurrent = (int) _gameConfigurationHandler.GetCurrentMonsterDifficulty(),
                NPCDifficultyNew = (int) _gameConfigurationHandler.GetNewMonsterDifficulty(),
                ItemSpawnRate = (int) _gameConfigurationHandler.GetSpawnRate()
            };
            _gameConfigServicesDb.CreateAsync(gameConfigurationPOCO);
            
            var gamePOCO = new GamePOCO {GameGUID = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId()};

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
                    {PlayerGUID = client[0], GameGUID = gamePOCO.GameGUID, GameGUIDAndPlayerGuid = gamePOCO.GameGUID + client[0], XPosition = playerX, YPosition = playerY};
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

        private void AddItemsToPlayer( string playerId, string gameId)
        {
            PlayerItemPOCO poco = new() {PlayerGUID = playerId, ItemName = ItemFactory.GetBandana().ItemName, GameGUID = gameId };
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

            // add name to players
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

            _worldService.DisplayWorld();
            _worldService.DisplayStats();
            _messageService.DisplayMessages();
        }
    }
}