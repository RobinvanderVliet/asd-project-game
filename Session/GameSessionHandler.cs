using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                startGameDTO = LoadSave();
            }
            else
            {
                startGameDTO = SetupGameHost();
            }

            SendGameSessionDTO(startGameDTO);
            _sessionHandler.SetGameStarted(true);
        }

        private StartGameDTO LoadSave()
        {
            StartGameDTO startGameDTO = new StartGameDTO();

            var playerService = ReturnPlayerService();
            var allPlayers = playerService.GetAllAsync();
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


        private ServicesDb<GamePOCO> ReturnGameService()
        {
            var dbConnection = new DbConnection();

            var gameRepository = new Repository<GamePOCO>(dbConnection);
            var gameService = new ServicesDb<GamePOCO>(gameRepository);

            return gameService;
        }

        private ServicesDb<PlayerPOCO> ReturnPlayerService()
        {
            var dbConnection = new DbConnection();

            var playerRepository = new Repository<PlayerPOCO>(dbConnection);
            var servicePlayer = new ServicesDb<PlayerPOCO>(playerRepository);

            return servicePlayer;
        }

        public StartGameDTO SetupGameHost()
        {
            StartGameDTO startGameDTO = new StartGameDTO();

            var gameService = ReturnGameService();
            var servicePlayer = ReturnPlayerService();

            var gamePOCO = new GamePOCO
            {
                GameGuid = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId(),
                Seed = _sessionHandler.GetSessionSeed()
            };
            gameService.CreateAsync(gamePOCO);

            List<string> allClients = _sessionHandler.GetAllClients();
            Dictionary<string, int[]> players = new Dictionary<string, int[]>();

            players = SetupPositionsNewPlayers(allClients, gamePOCO, servicePlayer);

            startGameDTO.GameGuid = gamePOCO.GameGuid;
            startGameDTO.PlayerLocations = players;

            return startGameDTO;
        }

        private Dictionary<string, int[]> SetupPositionsNewPlayers(List<string> allClients, GamePOCO gamePOCO,
            ServicesDb<PlayerPOCO> servicePlayer)
        {
            Dictionary<string, int[]> players = new Dictionary<string, int[]>();
            int playerX = 26; // spawn position
            int playerY = 11; // spawn position

            foreach (var clientId in allClients)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = playerX;
                playerPosition[1] = playerY;
                players.Add(clientId, playerPosition);
                Console.WriteLine(clientId);

                var tmpPlayer = new PlayerPOCO
                    {PlayerGuid = clientId, GameGuid = gamePOCO.GameGuid, XPosition = playerX, YPosition = playerY, GameGUIDAndPlayerGuid = gamePOCO.GameGuid + clientId};

                var insert = servicePlayer.CreateAsync(tmpPlayer);
                insert.Wait();

                playerX += 2; // spawn position + 2 each client
                playerY += 2; // spawn position + 2 each client
            }

    

            return players;
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

        private void AddPlayerToGameSession(StartGameDTO joinedPlayerDto)
        {
            _worldService.DisplayWorld();

            if (_clientController.GetOriginId() == joinedPlayerDto.ExistingPlayer.PlayerGuid)
            {
                _worldService.AddPlayerToWorld(
                    new WorldGeneration.Player("gerrit", joinedPlayerDto.ExistingPlayer.XPosition,
                        joinedPlayerDto.ExistingPlayer.YPosition,
                        CharacterSymbol.CURRENT_PLAYER, joinedPlayerDto.ExistingPlayer.PlayerGuid,
                        joinedPlayerDto.ExistingPlayer.Health,
                        joinedPlayerDto.ExistingPlayer.Stamina), true);
                
                foreach (var player in joinedPlayerDto.PlayerLocations)
                {
                    if (_clientController.GetOriginId() != joinedPlayerDto.ExistingPlayer.PlayerGuid)
                    {
                        _worldService.AddPlayerToWorld(
                            new WorldGeneration.Player("arie", player.Value[0], player.Value[1],
                                CharacterSymbol.ENEMY_PLAYER, player.Key), false);
                    }
                }
                
            }
            else
            {
                foreach (var player in joinedPlayerDto.PlayerLocations)
                {
                    if (_clientController.GetOriginId() == player.Key)
                    {
                        _worldService.AddPlayerToWorld(
                            new WorldGeneration.Player("gerrit", joinedPlayerDto.ExistingPlayer.XPosition,
                                joinedPlayerDto.ExistingPlayer.YPosition,
                                CharacterSymbol.CURRENT_PLAYER, joinedPlayerDto.ExistingPlayer.PlayerGuid,
                                joinedPlayerDto.ExistingPlayer.Health,
                                joinedPlayerDto.ExistingPlayer.Stamina), true);
                    }
                }

                _worldService.AddPlayerToWorld(
                    new WorldGeneration.Player("gerrit", joinedPlayerDto.ExistingPlayer.XPosition,
                        joinedPlayerDto.ExistingPlayer.YPosition,
                        CharacterSymbol.CURRENT_PLAYER, joinedPlayerDto.ExistingPlayer.PlayerGuid,
                        joinedPlayerDto.ExistingPlayer.Health,
                        joinedPlayerDto.ExistingPlayer.Stamina), false);
            }
         _worldService.DisplayWorld();
        }


        private void HandleStartGameSession(StartGameDTO startGameDTO)
        {
            _worldService.GenerateWorld(_sessionHandler.GetSessionSeed());

            if (_sessionHandler.GameStarted() && !_sessionHandler.GetSavedGame() || (_sessionHandler.GameStarted() && _sessionHandler.GetSavedGame()))
            {
                AddPlayerToGameSession(startGameDTO);
            }
            else if (_sessionHandler.GetSavedGame())
            {
                AddPlayerToWorldSavedGame(startGameDTO.SavedPlayers);
            }
            else
            {
                AddPlayersToNewGame(startGameDTO);
            }

            _worldService.DisplayWorld();
        }

        private void AddPlayersToNewGame(StartGameDTO startGameDTO)
        {
            _worldService.DisplayWorld();

            foreach (var player in startGameDTO.PlayerLocations)
            {
                if (_clientController.GetOriginId() == player.Key)
                {
                    var tmp = new DbConnection();

                    var clientHistoryRepository = new Repository<ClientHistoryPoco>(tmp);
                    var tmpClientHistory = new ServicesDb<ClientHistoryPoco>(clientHistoryRepository);
                    var tmpObject = new ClientHistoryPoco()
                        {PlayerId = player.Key, GameId = startGameDTO.GameGuid};
                    tmpClientHistory.CreateAsync(tmpObject);

                    _worldService.AddPlayerToWorld(
                        new WorldGeneration.Player("gerrit", player.Value[0], player.Value[1],
                            CharacterSymbol.CURRENT_PLAYER, player.Key), true);
                }
                else
                {
                    _worldService.AddPlayerToWorld(
                        new WorldGeneration.Player("arie", player.Value[0], player.Value[1],
                            CharacterSymbol.ENEMY_PLAYER, player.Key), false);
                }
            }

            _worldService.DisplayWorld();
        }

        private void AddPlayerToWorldSavedGame(List<PlayerPOCO> savedPlayers)
        {
            _worldService.DisplayWorld();

            foreach (var player in savedPlayers)
            {
                if (_clientController.GetOriginId() == player.PlayerGuid)
                {
                    _worldService.AddPlayerToWorld(
                        new WorldGeneration.Player("gerrit", player.XPosition, player.YPosition,
                            CharacterSymbol.CURRENT_PLAYER, player.PlayerGuid, player.Health, player.Stamina), true);
                }
                else
                {
                    _worldService.AddPlayerToWorld(
                        new WorldGeneration.Player("arie", player.XPosition, player.YPosition,
                            CharacterSymbol.ENEMY_PLAYER, player.PlayerGuid, player.Health, player.Stamina), true);
                }
            }

            _worldService.DisplayWorld();
        }
    }
}