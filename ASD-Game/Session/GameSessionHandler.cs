using System.Collections.Generic;
using ActionHandling;
using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using Items;
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
        private readonly IClientController _clientController;
        private readonly ISessionHandler _sessionHandler;
        private readonly IWorldService _worldService;
        private readonly IRelativeStatHandler _relativeStatHandler;
        private readonly IServicesDb<PlayerPOCO> _playerService;
        private readonly IServicesDb<GamePOCO> _gameService;
        private readonly IServicesDb<PlayerItemPOCO> _playerItemService;

        public GameSessionHandler(IClientController clientController, IWorldService worldService, ISessionHandler sessionHandler, IServicesDb<PlayerPOCO> playerService, IServicesDb<GamePOCO> gameService, IServicesDb<PlayerItemPOCO> playerItemService, IRelativeStatHandler relativeStatHandler)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.GameSession);
            _worldService = worldService;
            _sessionHandler = sessionHandler;
            _playerService = playerService;
            _gameService = gameService;
            _playerItemService = playerItemService;
            _relativeStatHandler = relativeStatHandler;
        }

        public void SendGameSession()
        {
            var StartGameDTO = SetupGameHost();
            SendGameSessionDTO(StartGameDTO);
        }

        public StartGameDTO SetupGameHost()
        {
            var gamePOCO = new GamePOCO {GameGuid = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId()};
            _gameService.CreateAsync(gamePOCO);

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
                    {PlayerGuid = clientId, GameGuid = gamePOCO.GameGuid, GameGUIDAndPlayerGuid = gamePOCO.GameGuid + clientId, XPosition = playerX, YPosition = playerY};
                _playerService.CreateAsync(tmpPlayer);
                AddItemsToPlayer(clientId, gamePOCO.GameGuid);

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
            PlayerItemPOCO poco = new() {PlayerGUID = playerId, ItemName = ItemFactory.GetBandana().ItemName, GameGUID = gameId };
            _playerItemService.CreateAsync(poco);

            poco = new() { PlayerGUID = playerId, ItemName = ItemFactory.GetKnife().ItemName, GameGUID = gameId };
            _playerItemService.CreateAsync(poco);
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
                    _worldService.AddPlayerToWorld(new WorldGeneration.Player("gerrit", player.Value[0], player.Value[1], CharacterSymbol.CURRENT_PLAYER, player.Key), true);
                }
                else
                {
                    var playerObject = new Player("arie", player.Value[0], player.Value[1], CharacterSymbol.ENEMY_PLAYER, player.Key);
                    _worldService.AddPlayerToWorld(playerObject, false);
                }
            }

            _worldService.DisplayWorld();
            _relativeStatHandler.SetCurrentPlayer(_worldService.GetCurrentPlayer());
            _relativeStatHandler.CheckStaminaTimer();
            _relativeStatHandler.CheckRadiationTimer();
        }
    }
}