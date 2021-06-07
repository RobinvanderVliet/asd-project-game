using System.Diagnostics.CodeAnalysis;
using ActionHandling.DTO;
using ASD_project.ActionHandling;
using ASD_project.DatabaseHandler.POCO;
using ASD_project.DatabaseHandler.Services;
using ASD_project.Network;
using ASD_project.Network.Enum;
using ASD_project.World.Services;
using Messages;
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
        private Mock<IDatabaseService<PlayerPOCO>> _mockedPlayerDatabaseService;
        private RelativeStatHandler _sut;
        private Mock<IMessageService> _mockedMessageService;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _mockedWorldService = new Mock<IWorldService>();
            _mockedPlayerDatabaseService = new Mock<IDatabaseService<PlayerPOCO>>();
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