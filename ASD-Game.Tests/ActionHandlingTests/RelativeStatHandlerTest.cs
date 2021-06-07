using System.Diagnostics.CodeAnalysis;
using ASD_Game.ActionHandling;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.Enum;
using ASD_Game.World.Services;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ASD_Game.Tests.ActionHandlingTests
{
    [ExcludeFromCodeCoverage]
    public class RelativeStatHandlerTest
    {
        private Mock<IClientController> _mockedClientController;
        private Mock<IWorldService> _mockedWorldService;
        private Mock<IDatabaseService<PlayerPoco>> _mockedPlayerDatabaseService;
        private RelativeStatHandler _sut;
        private Mock<IMessageService> _mockedMessageService;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedPlayerDatabaseService = new Mock<IDatabaseService<PlayerPoco>>();
            _mockedMessageService = new Mock<IMessageService>();

            _sut = new RelativeStatHandler(_mockedClientController.Object, _mockedWorldService.Object, _mockedPlayerDatabaseService.Object, _mockedMessageService.Object);
        }

        [Test]
        public void Test_SendStat_SendPayloadCorrectly()
        {
            //Arrange
            var dto = new RelativeStatDTO();
            dto.Id = "testId";
            dto.Stamina = 5;

            var payload = JsonConvert.SerializeObject(dto);

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("testId");
            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.RelativeStat));

            //Act
            _sut.SendStat(dto);
            
            //Assert
            _mockedClientController.Verify(mock => mock.GetOriginId(), Times.Once);
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.RelativeStat), Times.Once);
        }
    }
}