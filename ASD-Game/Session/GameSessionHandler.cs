using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
using System.Collections.Generic;
using ASD_project.World.Models;
using ASD_project.World.Models.Characters;
using ASD_project.World.Services;

namespace Session
{

    public class GameSessionHandler : IPacketHandler, IGameSessionHandler
    {
        private IClientController _clientController;
        private ISessionHandler _sessionHandler;
        private IWorldService _worldService;

        public GameSessionHandler(IClientController clientController, IWorldService worldService, ISessionHandler sessionHandler)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.GameSession);
            _worldService = worldService;
            _sessionHandler = sessionHandler;
        }
        
        public void SendGameSession()
        {
            var StartGameDTO = SetupGameHost();
            SendGameSessionDTO(StartGameDTO);
        }

        public StartGameDTO SetupGameHost()
        {
            var servicePlayer = new DatabaseService<PlayerPOCO>();
            var gameService = new DatabaseService<GamePOCO>();

            var gamePOCO = new GamePOCO {GameGUID = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId()};
            gameService.CreateAsync(gamePOCO);

            List<string> allClients = _sessionHandler.GetAllClients();
            Dictionary<string, int[]> players = new Dictionary<string, int[]>();

            // Have refactored into something a bit more exciting.
            
            int spawnSeed = _sessionHandler.GetSessionSeed();
            int playerX = spawnSeed % 50; // spawn position first person.
            int playerY = spawnSeed % 50;
            foreach (string clientId in allClients)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = playerX;
                playerPosition[1] = playerY;
                players.Add(clientId, playerPosition);
                var tmpPlayer = new PlayerPOCO
                    {PlayerGUID = clientId, GameGUID = gamePOCO.GameGUID, XPosition = playerX, YPosition = playerY};
                servicePlayer.CreateAsync(tmpPlayer);

                if (playerX % 2 == 0)
                {
                    playerX += spawnSeed % 3;
                }
                else
                {
                    playerX -= spawnSeed % 3;
                }
                if (playerY % 2 == 0)
                {
                    playerY += spawnSeed % 5;
                }
                else
                {
                    playerY -= spawnSeed % 5;
                }
            }

            StartGameDTO startGameDTO = new StartGameDTO();
            startGameDTO.GameGuid = _clientController.SessionId;
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

            // add name to players
            foreach (var player in startGameDTO.PlayerLocations)
            {
                if (_clientController.GetOriginId() == player.Key)
                {
                    // add name to players
                    _worldService.AddPlayerToWorld(new Player("gerrit", player.Value[0], player.Value[1], CharacterSymbol.CURRENT_PLAYER, player.Key), true);
                } 
                else 
                {
                    _worldService.AddPlayerToWorld(new Player("arie", player.Value[0], player.Value[1], CharacterSymbol.ENEMY_PLAYER, player.Key), false);
                }
            }

            _worldService.DisplayWorld();
        }
    }
}