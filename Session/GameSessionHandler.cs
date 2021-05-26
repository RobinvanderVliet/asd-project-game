using System;
using System.Collections.Generic;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using DataTransfer.DTO.Character;
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
        private IClientController _clientController;
        public IClientController ClientController { get => _clientController; }
        private ISessionHandler _sessionHandler;
        private IWorldService _worldService;
        
        public GameSessionHandler(IClientController clientController, IWorldService worldService, ISessionHandler sessionHandler)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.GameSession);
            _worldService = worldService;
            _sessionHandler = sessionHandler;
        }
        
        public void SendGameSession(ISessionHandler sessionHandler)
        {
            _sessionHandler = sessionHandler;
            var StartGameDTO = SetupGameHost();
            SendGameSessionDTO(StartGameDTO);
        }

        public StartGameDTO SetupGameHost()
        {
            var dbConnection = new DbConnection();

            var playerRepository = new Repository<PlayerPOCO>(dbConnection);
            var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);
            var gameRepository = new Repository<GamePOCO>(dbConnection);
            var gameService = new ServicesDb<GamePOCO>(gameRepository);

            string gameGuid = Guid.NewGuid().ToString();
            var gamePOCO = new GamePOCO {GameGuid = gameGuid, PlayerGUIDHost = _clientController.GetOriginId()};
            gameService.CreateAsync(gamePOCO);
  
            List<string> allClients = _sessionHandler.GetAllClients();
            Dictionary<string, int[]> players = new Dictionary<string, int[]>();
            
            // TODO Needs to be refactored to something random in construction; this was for testing
            int playerX = 26; // spawn position
            int playerY = 11; // spawn position
            foreach (string element in allClients)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = playerX;
                playerPosition[1] = playerY;
                players.Add(element, playerPosition);
                var tmpPlayer = new PlayerPOCO
                    {PlayerGuid = element, GameGuid = gamePOCO.GameGuid, XPosition = playerX, YPosition = playerY};
                servicePlayer.CreateAsync(tmpPlayer);

                playerX += 2; // spawn position + 2 each client
                playerY += 2; // spawn position + 2 each client
            }

            StartGameDTO startGameDTO = new StartGameDTO();
            startGameDTO.GameGuid = gameGuid;
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

        private void HandleStartGameSession(StartGameDTO startGameDTO)
        {
            _worldService.GenerateWorld(_sessionHandler.GetSessionSeed());

            foreach (var player in startGameDTO.PlayerLocations)
            {
                if (_clientController.GetOriginId() == player.Key) 
                {
                    _worldService.AddCharacterToWorld(new MapCharacterDTO(player.Value[0], player.Value[1], player.Key, startGameDTO.GameGuid, CharacterSymbol.CURRENT_PLAYER), true);
                } 
                else 
                {
                    _worldService.AddCharacterToWorld(new MapCharacterDTO(player.Value[0], player.Value[1], player.Key, startGameDTO.GameGuid,CharacterSymbol.ENEMY_PLAYER), false);
                }
            }
            _worldService.DisplayWorld();

            if (_clientController.IsBackupHost)
            {
                SaveInBackupDatabase(startGameDTO);
            }
        }

        private void SaveInBackupDatabase(StartGameDTO startGameDTO)
        {
            var dbConnection = new DbConnection();

            //create new backup repositories.
            var playerRepository = new Repository<PlayerPOCO>(dbConnection);
            var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);
            var gameRepository = new Repository<GamePOCO>(dbConnection);
            var gameService = new ServicesDb<GamePOCO>(gameRepository);

            //Game
            var gamePOCO = new GamePOCO { GameGuid = startGameDTO.GameGuid, PlayerGUIDHost = _sessionHandler.GetAllClients()[0]};
            gameService.CreateAsync(gamePOCO);

            //Players
            foreach (KeyValuePair<string, int[]> player in startGameDTO.PlayerLocations)
            {
                servicePlayer.CreateAsync(
                    new PlayerPOCO{ PlayerGuid = player.Key, GameGuid = gamePOCO.GameGuid, XPosition = player.Value[0], YPosition = player.Value[1] }
                    );
            }
            Console.WriteLine("backup goes brrrrrrrrrrrrrrrrrrr");
        }
    }
}