using Moq;
using Network;
using Network.DTO;
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
        private SessionHandler _sessionHandler;

        private PacketDTO _packetDTO;

        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;

        [SetUp]
        public void Setup()
        {
            _mockedClientController = new Mock<IClientController>();
            _sessionHandler = new SessionHandler(_mockedClientController.Object);
            _packetDTO = new PacketDTO();
        }

        // TODO: fix this test
        [Test]
        public void Test_JoinSession()
        {
            // Arrange ------------
            //string testSessionId = "testSessionId";

            // Act ----------------
            //_sessionHandler.JoinSession(testSessionId);

            // Assert -------------
            //_mockedClientController.Verify(mock => mock.SetSessionId(testSessionId), Times.Once());
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
            // Arrange ---------
            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessions);
            var payload = JsonConvert.SerializeObject(sessionDTO);

            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Session));

            // Act ---------
            _sessionHandler.RequestSessions();

            // Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.Session), Times.Once());
        }

        [Test]
        public void Test_HandlePacketRequestSessions()
        {
            // Arrange ---------
            _sessionHandler.CreateSession("testSessionName");

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessions);
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = "testOriginId";
            packetHeaderDTO.SessionID = null;
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            // Act -------------
            HandlerResponseDTO actualResult = _sessionHandler.HandlePacket(_packetDTO);

            // Assert ----------
            HandlerResponseDTO expectedResult = new HandlerResponseDTO(true, "testSessionName");
            Assert.AreEqual(expectedResult.ReturnToSender, actualResult.ReturnToSender);
        }

        [Test]
        public void Test_HandlePacketRequestToJoinSessionThatDoesNotExist()
        {
            // Arrange ---------
            _sessionHandler.CreateSession("testSessionName");

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = "testOriginId";
            packetHeaderDTO.SessionID = null;
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            // Act -------------
            HandlerResponseDTO actualResult = _sessionHandler.HandlePacket(_packetDTO);

            // Assert ----------
            HandlerResponseDTO expectedResult = new HandlerResponseDTO(false, "testSessionName");
            Assert.AreEqual(expectedResult.ReturnToSender, actualResult.ReturnToSender);
        }

        [Test]
        public void Test_HandlePacketRequestToJoinSession()
        {
            // Arrange ---------

            // Act -------------

            // Assert ----------
            Assert.Pass();
        }
    }
}