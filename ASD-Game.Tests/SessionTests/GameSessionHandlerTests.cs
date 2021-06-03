using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Session.DTO;
using WorldGeneration;

namespace Session.Tests
{
    [ExcludeFromCodeCoverage]
    public class GameSessionHandlerTests
    {
        private GameSessionHandler _sut;

        private PacketDTO _packetDTO;

        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;
        private Mock<IWorldService> _mockedWorldService;
        private Mock<ISessionHandler> _mockedsessionHandler;
        private Mock<IDatabaseService<GamePOCO>> _mockedGamePOCOServices;
        private Mock<IDatabaseService<PlayerPOCO>> _mockedPlayerPOCOServices;
        private IList<GamePOCO> _InMemoryDatabaseGame;
        private IList<PlayerPOCO> _InMemoryDatabasePlayer = new List<PlayerPOCO>();
        private IDatabaseService<PlayerPOCO> _services;


        [SetUp]
        public void Setup()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            _mockedClientController = new Mock<IClientController>();
            _mockedPlayerPOCOServices = new Mock<IDatabaseService<PlayerPOCO>>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedsessionHandler = new Mock<ISessionHandler>();

            _sut = new GameSessionHandler(_mockedClientController.Object, _mockedWorldService.Object,
                _mockedsessionHandler.Object, _mockedGamePOCOServices.Object, _mockedPlayerPOCOServices.Object);
            _packetDTO = new PacketDTO();
        }

        [Test]
        public void Test_start_session()
        {
            //arrange
            List<string> allClients = new List<string>();
            allClients.Add("a");
            allClients.Add("b");
            allClients.Add("c");

            Dictionary<string, int[]> players = new Dictionary<string, int[]>();

            int playerX = 26; // spawn position
            int playerY = 11; // spawn position
            foreach (string element in allClients)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = playerX;
                playerPosition[1] = playerY;
                players.Add(element, playerPosition);
                playerX += 2; // spawn position + 2 each client
                playerY += 2; // spawn position + 2 each client
            }

            StartGameDTO startGameDto = new StartGameDTO();
            startGameDto.PlayerLocations = players;

            // Act ---------
            _sut.SendGameSession();

            var payload = JsonConvert.SerializeObject(startGameDto);

            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.GameSession));

            // Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.GameSession), Times.Once());
        }

        [Test]
        public void Test_GameSession_HandlePacket_SendsPacket()
        {
            string gameID = "gameId";

            PlayerPOCO player1 = new PlayerPOCO
            {
                Health = 10,
                Stamina = 100,
                GameGuid = gameID,
                XPosition = 10,
                YPosition = 20,
                PlayerGuid = "idPlayer1"
            };

            // Arrange ---------
            List<string> allClients = new List<string>();
            allClients.Add("a");
            allClients.Add("b");
            allClients.Add("c");


            Dictionary<string, int[]> players = new Dictionary<string, int[]>();

            int playerX = 26; // spawn position
            int playerY = 11; // spawn position
            foreach (string element in allClients)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = playerX;
                playerPosition[1] = playerY;
                players.Add(element, playerPosition);
                playerX += 2; // spawn position + 2 each client
                playerY += 2; // spawn position + 2 each client
            }

            _mockedsessionHandler.Setup(x => x.GetAllClients()).Returns(allClients);
            _sut.SendGameSession();

            StartGameDTO gameDto = new StartGameDTO();
            gameDto.PlayerLocations = players;


            var payload = JsonConvert.SerializeObject(gameDto);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = "testOriginId";
            packetHeaderDTO.SessionID = null;
            packetHeaderDTO.PacketType = PacketType.GameSession;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            HandlerResponseDTO expectedResult = new HandlerResponseDTO(SendAction.SendToClients, "testSessionName");
            Assert.AreEqual(expectedResult.Action, actualResult.Action);
        }
    }
}