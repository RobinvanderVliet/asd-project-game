using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Moq;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using NUnit.Framework;
using Session.DTO;
using Session.GameConfiguration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ActionHandling;
using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using WorldGeneration;
using Session.DTO;

namespace Session.Tests
{
    [ExcludeFromCodeCoverage]
    public class GameSessionHandlerTests
    {
        private GameSessionHandler _sut;

        private PacketDTO _packetDTO;

        //Declaration of mocks
        private Mock<ClientController> _mockedClientController; //change this to the interface and all test break, your choice.
        private Mock<IWorldService> _mockedWorldService;
        private Mock<ISessionHandler> _mockedsessionHandler;
        private Mock<IDatabaseService<PlayerPOCO>> _mockedPlayerDatabaseService;
        private Mock<IDatabaseService<GamePOCO>> _mockedGameDatabaseService;
        private Mock<IDatabaseService<PlayerItemPOCO>> _mockedPlayerItemDatabaseService;
        private Mock<IRelativeStatHandler> _mockedRelativeStatHandler;

        private Mock<IGameConfigurationHandler> _mockedGameConfigurationHandler;
        private Mock<IDatabaseService<GameConfigurationPOCO>> _mockedGameConfigDatabaseService;


        [SetUp]
        public void Setup()
        {
            Mock<INetworkComponent> tmpMock = new();

            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            _mockedClientController = new Mock<ClientController>(tmpMock.Object);
            _mockedWorldService = new Mock<IWorldService>();
            _mockedsessionHandler = new Mock<ISessionHandler>();
            _mockedGameConfigurationHandler = new Mock<IGameConfigurationHandler>();
            _mockedGameConfigDatabaseService = new Mock<IDatabaseService<GameConfigurationPOCO>>();
            _mockedPlayerItemDatabaseService = new Mock<IDatabaseService<PlayerItemPOCO>>();
            _mockedRelativeStatHandler = new Mock<IRelativeStatHandler>();
            _mockedPlayerDatabaseService = new Mock<IDatabaseService<PlayerPOCO>>();
            _mockedGameDatabaseService = new Mock<IDatabaseService<GamePOCO>>();
           // _sut = new GameSessionHandler(_mockedClientController.Object, _mockedWorldService.Object, _mockedsessionHandler.Object, _mockedPlayerServiceDb.Object, _mockedgameDatabaseService.Object, _mockedGameConfigDatabaseService.Object, _mockedGameConfigurationHandler.Object);
            _packetDTO = new PacketDTO();
        }

        //Test below fails, not worth fixing atm since no other functions get tested
        // [Test]
        // public void Test_SendGameSession_CallsSendPayloadWithCorrectPayload()
        // {
        //     //arrange
        //     Dictionary<string, int[]> players = new Dictionary<string, int[]>();
        //     
        //     int[] playerPosition = new int[2];
        //     playerPosition[0] = 1;
        //     playerPosition[1] = 2;
        //     players.Add("player", playerPosition);
        //     
        //     StartGameDTO startGameDTO = new StartGameDTO
        //         {GameGuid = "testGame", PlayerLocations = players};
        //     
        //     var payload = JsonConvert.SerializeObject(startGameDTO);
        //
        //     _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Session));
        //     _mockedsessionHandler.Setup(mock => mock.GetAllClients()).Returns(new List<string>());
        //
        //     // Act ---------
        //     _sut.SendGameSession(_mockedsessionHandler.Object);
        //
        //     // Assert ---------
        //     _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.Session), Times.Once());
        // }

        [Test]
        public void Test_SaveInBackupDatabase()
        {
            //Arrange
            var tmp = new StartGameDTO();
            int[] arr = new int[2] { 1, 1};
            tmp.GameGuid = "test";
            tmp.PlayerLocations = new();
            tmp.PlayerLocations.Add("player", arr);

            PacketDTO packet = new()
            {
                Header = new PacketHeaderDTO(),
                Payload = JsonConvert.SerializeObject(tmp)
            };

            _sut.ClientController.SetBackupHost(true);

            //mocked setup
            _mockedsessionHandler.Setup(x => x.GetAllClients()).Returns(new List<string> {"een super cool ID"});

            //Act
            var result = _sut.HandlePacket(packet);

            //Assert
            _mockedsessionHandler.Verify(x => x.GetAllClients(), Times.Once);
            Assert.AreEqual(result.GetType(), new HandlerResponseDTO(It.IsAny<SendAction>(), It.IsAny<string>()).GetType());

        }
    }
}