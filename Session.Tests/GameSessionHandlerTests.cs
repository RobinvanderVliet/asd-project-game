using System;
using System.Collections.Generic;
using System.IO;
using DataTransfer.DTO.Character;
using Moq;
using Network;
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

        [SetUp]
        public void Setup()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            _mockedClientController = new Mock<IClientController>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedsessionHandler = new Mock<ISessionHandler>();
            _sut = new GameSessionHandler(_mockedClientController.Object,_mockedWorldService.Object,_mockedsessionHandler.Object);
            _packetDTO = new PacketDTO();
        }
        
        [Test]
     
        public void Test_start_session()
        {
            //arrange
            Dictionary<string, int[]> players = new Dictionary<string, int[]>();
            
            int[] playerPosition = new int[2];
            playerPosition[0] = 1;
            playerPosition[1] = 2;
            players.Add("player", playerPosition);
            
            StartGameDto startGameDto = new StartGameDto
                {GameGuid = "testGame", PlayerLocations = players};
            
            var payload = JsonConvert.SerializeObject(startGameDto);

            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Session));

            // Act ---------
            _sut.SendGameSession(_mockedsessionHandler.Object);

            // Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.Session), Times.Once());
          
      
        }

        
    }
}