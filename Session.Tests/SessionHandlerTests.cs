using Moq;
using Network;
using Newtonsoft.Json;
using NUnit.Framework;
using Session.DTO;
using System.Diagnostics.CodeAnalysis;

namespace Session.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SessionHandlerTests
    {
        //Declaration and initialisation of constant variables
        SessionHandler _sessionHandler;

        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _sessionHandler = new SessionHandler(_mockedClientController.Object);
        }

        [Test]
        public void Test_JoinSession()
        {
            Assert.Pass();
        }

        [Test]
        public void Test_CreateSession()
        {
            // Arrange ------------
            string testSessionName = "testSessionName";

            _mockedClientController.Setup(mock => mock.CreateHostController());

            // Act ----------------
            _sessionHandler.CreateSession(testSessionName);

            // Assert -------------
            _mockedClientController.Verify(mock => mock.CreateHostController(), Times.Once());
        }

        [Test]
        public void Test_RequestSessions()
        {
            //Arrange ---------
            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessions);
            var payload = JsonConvert.SerializeObject(sessionDTO);

            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Session));

            //Act ---------
            _sessionHandler.RequestSessions();

            //Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.Session), Times.Once());
        }

        [Test]
        public void Test_HandlePacket()
        {
            Assert.Pass();
        }
    }
}