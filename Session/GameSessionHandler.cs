using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
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
        private ISessionHandler _sessionHandler;
        private IWorldService _worldService;

        public GameSessionHandler(IClientController clientController, IWorldService worldService,
            ISessionHandler sessionHandler)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.GameSession);
            _worldService = worldService;
            _sessionHandler = sessionHandler;
        }

        public void SendGameSession()
        {
            StartGameDTO startGameDTO = new StartGameDTO();
            if (_sessionHandler.GetSavedGame())
            {
                Console.WriteLine("this is a saved game.");
                startGameDTO = LoadSave(_sessionHandler.GetSavedGameName());
            }
            else
            {
                startGameDTO = SetupGameHost();
            }
            SendGameSessionDTO(startGameDTO);
        }

        private StartGameDTO LoadSave(string gameGuid)
        {
            var dbConnection = new DbConnection();

            var playerRepository = new Repository<PlayerPOCO>(dbConnection);
            var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);
            var gameRepository = new Repository<GamePOCO>(dbConnection);
            var gameService = new ServicesDb<GamePOCO>(gameRepository);

            var allPlayers = servicePlayer.GetAllAsync();
            allPlayers.Wait();
            StartGameDTO startGameDto = new StartGameDTO();
            startGameDto.SavedPlayers = allPlayers.Result.Where(x => x.GameGuid == gameGuid).ToList();

            Dictionary<string, int[]> players = new Dictionary<string, int[]>();

            foreach (var element in startGameDto.SavedPlayers)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = element.XPosition;
                playerPosition[1] = element.YPosition;
                players.Add(element.PlayerGuid, playerPosition);
            }
            
            return startGameDto;
        }

        public StartGameDTO SetupGameHost()
        {
            var dbConnection = new DbConnection();

            var playerRepository = new Repository<PlayerPOCO>(dbConnection);
            var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);
            var gameRepository = new Repository<GamePOCO>(dbConnection);
            var gameService = new ServicesDb<GamePOCO>(gameRepository);

            var gamePOCO = new GamePOCO {GameGuid = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId()};
            gameService.CreateAsync(gamePOCO);

            List<string> allClients = _sessionHandler.GetAllClients();
            Dictionary<string, int[]> players = new Dictionary<string, int[]>();

            // Needs to be refactored to something random in construction; this was for testing
            int playerX = 26; // spawn position
            int playerY = 11; // spawn position
            foreach (string clientId in allClients)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = playerX;
                playerPosition[1] = playerY;
                players.Add(clientId, playerPosition);
                var tmpPlayer = new PlayerPOCO
                    {PlayerGuid = clientId, GameGuid = gamePOCO.GameGuid, XPosition = playerX, YPosition = playerY};
                servicePlayer.CreateAsync(tmpPlayer);

                playerX += 2; // spawn position + 2 each client
                playerY += 2; // spawn position + 2 each client
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

            if (_sessionHandler.GetSavedGame() && _clientController.IsHost())
            {
                var temp = new DbConnection();

                var playerRepository = new Repository<PlayerPOCO>(temp);
                var playerService = new ServicesDb<PlayerPOCO>(playerRepository);
                var result = playerService.GetAllAsync();
                result.Wait();
                var resultsult = result.Result.Where(x => x.GameGuid == startGameDTO.GameGuid);

                startGameDTO.SavedPlayers = new List<PlayerPOCO>();

                foreach (var player in resultsult)
                {
                    startGameDTO.SavedPlayers.Add(player);

                    if (_clientController.GetOriginId() == player.PlayerGuid)
                    {
                        _worldService.AddPlayerToWorld(new WorldGeneration.Player("gerrit", player.XPosition, player.YPosition, CharacterSymbol.CURRENT_PLAYER, player.PlayerGuid, player.Health, player.Stamina), true);
                    }
                    else
                    {
                        _worldService.AddPlayerToWorld(new WorldGeneration.Player("arie", player.XPosition, player.YPosition, CharacterSymbol.ENEMY_PLAYER, player.PlayerGuid, player.Health, player.Stamina), false);
                    }
                }
            } else
            {
                foreach (var player in startGameDTO.PlayerLocations)
                {
                    if (_clientController.GetOriginId() == player.Key)
                    {
                        var tmp = new DbConnection();

                        var clientHistoryRepository = new Repository<ClientHistoryPoco>(tmp);
                        var tmpClientHistory = new ServicesDb<ClientHistoryPoco>(clientHistoryRepository);

                        var tmpObject = new ClientHistoryPoco() { PlayerId = player.Key, GameId = startGameDTO.GameGuid };

                        tmpClientHistory.CreateAsync(tmpObject);

                        _worldService.AddPlayerToWorld(new WorldGeneration.Player("gerrit", player.Value[0], player.Value[1], CharacterSymbol.CURRENT_PLAYER, player.Key), true);
                    }
                    else
                    {
                        _worldService.AddPlayerToWorld(new WorldGeneration.Player("arie", player.Value[0], player.Value[1], CharacterSymbol.ENEMY_PLAYER, player.Key), false);
                    }
                }
            }

            _worldService.DisplayWorld();
        }

        
    }
}