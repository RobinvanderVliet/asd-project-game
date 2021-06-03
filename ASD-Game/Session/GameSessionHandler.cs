using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
using System;
using System.Collections.Generic;
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
        private Random random = new Random();

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

            var gamePOCO = new GamePOCO { GameGUID = _clientController.SessionId, PlayerGUIDHost = _clientController.GetOriginId() };
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
                { PlayerGUID = clientId, GameGUID = gamePOCO.GameGUID, XPosition = playerX, YPosition = playerY };
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

            // add name to player
            foreach (var player in startGameDTO.PlayerLocations)
            {
                if (_clientController.GetOriginId() == player.Key)
                {
                    // add name to players
                    _worldService.AddPlayerToWorld(new WorldGeneration.Player("gerrit", player.Value[0], player.Value[1], CharacterSymbol.CURRENT_PLAYER, player.Key), true);
                }
                else
                {
                    _worldService.AddPlayerToWorld(new WorldGeneration.Player("arie", player.Value[0], player.Value[1], CharacterSymbol.ENEMY_PLAYER, player.Key), false);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                Monster newMonster = new Monster("Zombie", random.Next(12, 25), random.Next(12, 25), CharacterSymbol.TERMINATOR, "monst" + i);
                setBrain(newMonster);
                _worldService.AddCreatureToWorld(newMonster);
            }

            _worldService.DisplayWorld();
        }

        private void setBrain(Monster monster)
        {
            if (random.Next(0, 100) > 5)
            {
                ICharacterStateMachine CSM = new MonsterStateMachine(monster._monsterData, null);
                monster._monsterStateMachine = CSM;
            }
            else
            {
                if (_sessionHandler.trainingScenario.brainTransplant() != null)
                {
                    monster.brain = _sessionHandler.trainingScenario.brainTransplant();
                }
            }
        }
    }
}