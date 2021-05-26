using System;
using System.Collections.Generic;
using System.IO;
using DatabaseHandler.Poco;
using DatabaseHandler.Repository;
using Moq;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using NUnit.Framework;
using Session.DTO;
using WorldGeneration;

namespace Session.Tests
{
    public class GameSessionHandlerTests
    {
        private GameSessionHandler _sut;

        private PacketDTO _packetDTO;

        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;
        private Mock<IWorldService> _mockedWorldService;
        private Mock<ISessionHandler> _mockedsessionHandler;
        private Mock<IGuidService> _mockedGuidService;

        [SetUp]
        public void Setup()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            _mockedClientController = new Mock<IClientController>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedsessionHandler = new Mock<ISessionHandler>();
            _mockedGuidService = new Mock<IGuidService>();
            _sut = new GameSessionHandler(_mockedClientController.Object,_mockedWorldService.Object,_mockedsessionHandler.Object, _mockedGuidService.Object);
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
            
            var GUID = new Guid("16346707-31d3-4566-b87d-a0b45803b7ab");
            
            StartGameDto startGameDto = new StartGameDto();
            startGameDto.GameGuid = GUID.ToString();
            startGameDto.PlayerLocations = players;

            _mockedsessionHandler.Setup(x => x.GetAllClients()).Returns(allClients);
            _mockedGuidService.Setup(x => x.NewGuid()).Returns(GUID);
            
            // Act ---------
            _sut.SendGameSession(_mockedsessionHandler.Object);

            var payload = JsonConvert.SerializeObject(startGameDto);

            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.GameSession));

            // Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.GameSession), Times.Once());
        }

        [Test]
        public void Test_GameSession_HandlePacket()
        {
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
            _sut.SendGameSession(_mockedsessionHandler.Object);
               

                StartGameDto gameDto = new StartGameDto();
                gameDto.PlayerLocations = players;
                gameDto.GameGuid = Guid.NewGuid().ToString();
                
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