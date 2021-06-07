using System.Diagnostics.CodeAnalysis;
using ASD_project.ActionHandling;
using ASD_project.DatabaseHandler.POCO;
using ASD_project.DatabaseHandler.Services;
using ASD_project.Network;
using ASD_project.Network.Enum;
using ASD_project.World.Models.Characters;
using ASD_project.World.Services;
using Messages;
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
        private Mock<IDatabaseService<PlayerPOCO>> _mockedServicesDb;
        private Mock<IMessageService> _mockedMessageService;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedServicesDb = new Mock<IDatabaseService<PlayerPOCO>>();
            _mockedMessageService = new Mock<IMessageService>();
            _sut = new MoveHandler(_mockedClientController.Object, _mockedWorldService.Object, _mockedServicesDb.Object, _mockedMessageService.Object);
        }

        [TestCase("up")]
        [TestCase("down")]
        [TestCase("left")]
        [TestCase("right")]
        [Test]
        public void Test_SendMove_(string move)
        {
            //arrange
            var direction = move;
            int steps = 5;
            int x = 26;
            int y = 11;
            
            Player player = new Player("test", x, y, "#", "test2");
            
            _mockedWorldService.Setup(mock => mock.GetCurrentPlayer()).Returns(player);
            _mockedClientController.Setup(mock => mock.SendPayload(It.IsAny<string>(), PacketType.Move));
            
            //act
            _sut.SendMove(direction, steps);
            
            //assert
            _mockedWorldService.Verify(mock => mock.GetCurrentPlayer(), Times.Once);
            _mockedClientController.Verify(mock => mock.SendPayload(It.IsAny<string>(), PacketType.Move), Times.Once);
        }
    }
}