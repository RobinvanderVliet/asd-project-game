using System.Collections.Generic;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using Items;
using DatabaseHandler.Repository;
using Items;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
using Session.GameConfiguration;
using System;
using System.Collections.Generic;
using UserInterface;
using DatabaseHandler.Services;
using WorldGeneration;
using WorldGeneration.Models;
using Messages;

namespace Session
{

    public class GameSessionHandler : IPacketHandler, IGameSessionHandler
    {
        private readonly IClientController _clientController;
        private readonly ISessionHandler _sessionHandler;
        private readonly IWorldService _worldService;
        
        private readonly IMessageService _messageService;
        private readonly IGameConfigurationHandler _gameConfigurationHandler;
        private readonly IScreenHandler _screenHandler;
        
        private readonly IDatabaseService<PlayerPOCO> _playerServicesDb;
        private readonly IDatabaseService<GamePOCO> _gameServicesDb;
        private readonly IDatabaseService<GameConfigurationPOCO> _gameConfigServicesDb;
        private readonly IDatabaseService<PlayerItemPOCO> _playerItemServicesDb;

       

        public GameSessionHandler(IClientController clientController, IWorldService worldService, ISessionHandler sessionHandler, IMessageService messageService, IDatabaseService<PlayerPOCO> playerServicesDb, IDatabaseService<GamePOCO> gameServicesDb, IDatabaseService<GameConfigurationPOCO> gameConfigServicesDb, IDatabaseService<PlayerItemPOCO> playerItemServicesDb, IGameConfigurationHandler gameConfigurationHandler, IScreenHandler screenHandler)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.GameSession);
            _worldService = worldService;
            _messageService = messageService;
            _playerServicesDb = playerServicesDb;
            _gameServicesDb = gameServicesDb;
            _gameConfigServicesDb = gameConfigServicesDb;
            _playerItemServicesDb = playerItemServicesDb;
            _gameConfigurationHandler = gameConfigurationHandler;
            _screenHandler = screenHandler;
            _sessionHandler = sessionHandler;
            _messageService = messageService;
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
            
            var gamePOCO = new GamePOCO {GameGuid = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId()};
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
                    {PlayerGuid = client[0], GameGUIDAndPlayerGuid = gamePOCO.GameGuid+_clientController.GetOriginId(), PlayerName = client[1], GameGuid = gamePOCO.GameGuid, XPosition = playerX, YPosition = playerY, Stamina = 100, Health = 100};
                _playerServicesDb.CreateAsync(tmpPlayer);
                var playerHelmet = new PlayerItemPOCO()
                {
                   GameGUID = gamePOCO.GameGuid, PlayerGUID = client[0], ItemName = ItemFactory.GetBandana().ItemName, ArmorPoints = ItemFactory.GetBandana().ArmorProtectionPoints
                };
                var playerWeapon = new PlayerItemPOCO(){
                    GameGUID = gamePOCO.GameGuid, PlayerGUID = client[0], ItemName = ItemFactory.GetKnife().ItemName
                };
                _playerItemServicesDb.CreateAsync(playerHelmet);
                _playerItemServicesDb.CreateAsync(playerWeapon);
                
                

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