using System.Diagnostics.CodeAnalysis;
using Moq;
using Network;
using NUnit.Framework;
using WorldGeneration;
using DatabaseHandler.Services;
using DatabaseHandler.POCO;

namespace ActionHandling.Tests
{
    [ExcludeFromCodeCoverage]
    public class MoveHandlerTest
    {
        private MoveHandler _sut;
        private Mock<IClientController> _mockedClientController;
        private Mock<IWorldService> _mockedWorldService;
        private Mock<IServicesDb<PlayerPOCO>> _mockedServicesDb;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedServicesDb = new Mock<IServicesDb<PlayerPOCO>>();
            _sut = new MoveHandler(_mockedClientController.Object, _mockedWorldService.Object, _mockedServicesDb.Object);
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