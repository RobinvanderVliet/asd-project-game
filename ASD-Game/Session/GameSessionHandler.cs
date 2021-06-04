using System.Collections.Generic;
using ActionHandling;
using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using Items;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
using Session.GameConfiguration;
using System;
using System.Collections.Generic;
using UserInterface;
using WorldGeneration;
using WorldGeneration.Models;
using Messages;

namespace Session
{
    public class GameSessionHandler : IPacketHandler, IGameSessionHandler
    {
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
            IMessageService messageService 
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
            _gameConfigDatabaseService.CreateAsync(gameConfigurationPOCO);
            
            var gamePOCO = new GamePOCO {GameGuid = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId()};

            _gameDatabaseService.CreateAsync(gamePOCO);
  
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
                    {PlayerGuid = client[0], GameGuid = gamePOCO.GameGuid, GameGUIDAndPlayerGuid = gamePOCO.GameGuid + client[0], XPosition = playerX, YPosition = playerY};
                _playerDatabaseService.CreateAsync(tmpPlayer);
                AddItemsToPlayer(client[0], gamePOCO.GameGuid);

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
            _screenHandler.TransitionTo(new GameScreen());
            var startGameDTO = JsonConvert.DeserializeObject<StartGameDTO>(packet.Payload);
            HandleStartGameSession(startGameDTO);
            return new HandlerResponseDTO(SendAction.SendToClients, null);
        }

        private void HandleStartGameSession(StartGameDTO startGameDTO)
        {
            _worldService.GenerateWorld(_sessionHandler.GetSessionSeed());
            Player currentPlayer = null;
            // add name to players
            foreach (var player in startGameDTO.PlayerLocations)
            {
                if (_clientController.GetOriginId() == player.Key)
                {
                    // add name to players
                    currentPlayer = new Player("gerrit", player.Value[0], player.Value[1],
                        CharacterSymbol.CURRENT_PLAYER, player.Key);
                    _worldService.AddPlayerToWorld(currentPlayer, true);
                    
                }
                else
                {
                    var playerObject = new Player("arie", player.Value[0], player.Value[1], CharacterSymbol.ENEMY_PLAYER, player.Key);
                    _worldService.AddPlayerToWorld(playerObject, false);
                }
            }

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
        }
    }
}