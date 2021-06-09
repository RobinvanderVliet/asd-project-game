using System;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.ActionHandling;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Services;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.ActionHandlingTests
{
    [ExcludeFromCodeCoverage]
    public class MoveHandlerTest
    {
        private MoveHandler _sut;
        private Mock<IClientController> _mockedClientController;
        private Mock<IWorldService> _mockedWorldService;
        private Mock<IDatabaseService<PlayerPOCO>> _mockedPlayerDatabaseService;
        private Mock<IMessageService> _mockedMessageService;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedPlayerDatabaseService = new Mock<IDatabaseService<PlayerPOCO>>();
            _mockedMessageService = new Mock<IMessageService>();
            _sut = new MoveHandler(_mockedClientController.Object, _mockedWorldService.Object, _mockedPlayerDatabaseService.Object, _mockedMessageService.Object);
        }

        [TestCase("up")]
        [TestCase("down")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void Test_SendMove_(string move)
        {
            // arrange
            var direction = move;
            int steps = 5;
            int x = 26;
            int y = 11;
            
            Player player = new Player("test", x, y, "#", "test2");
            
            _mockedWorldService.Setup(mock => mock.GetCurrentPlayer()).Returns(player);
            _mockedClientController.Setup(mock => mock.SendPayload(It.IsAny<string>(), PacketType.Move));
            
            // act
            _sut.SendMove(direction, steps);
            
            // assert
            _mockedClientController.Verify(mock => mock.SendPayload(It.IsAny<string>(), PacketType.Move), Times.Once);
        }

        [TestCase("up")]
        [TestCase("down")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void Test_DeadMove_(string move)
        {
            // arrange
            var direction = move;
            int steps = 5;
            int x = 26;
            int y = 11;

            Player player = new Player("test", x, y, "#", "test2");

            _mockedWorldService.Setup(mock => mock.GetCurrentPlayer()).Returns(player);
            _mockedClientController.Setup(mock => mock.SendPayload(It.IsAny<string>(), PacketType.Move)); 
            _mockedWorldService.Setup(mock => mock.IsDead(new Player())).Returns(true);

            // act
            _sut.SendMove(direction, steps);

            // assert
            _mockedClientController.Verify(mock => mock.SendPayload(It.IsAny<string>(), PacketType.Move), Times.Once);
        }

        [Test]
        public void Test_HandlePacket()
        {
            // arrange
            PacketDTO packetDTO = new PacketDTO();

            // act
            _sut.HandlePacket(packetDTO);

            // assert

        }

        [Test]
        public void Test_HandleMove()
        {
            // arrange
            MoveDTO moveDTO = new MoveDTO();
            bool handleInDatabase = false;

            // act
            _sut.HandleMove(moveDTO, handleInDatabase);

            // assert

        }

    }
}