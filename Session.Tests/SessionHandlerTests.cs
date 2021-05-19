using Moq;
using Network;
using Network.DTO;
using Newtonsoft.Json;
using NUnit.Framework;
using Session.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Session.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SessionHandlerTests
    {
        //Declaration and initialisation of constant variables
        private SessionHandler _sut;

        private PacketDTO _packetDTO;

        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;

        [SetUp]
        public void Setup()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            _mockedClientController = new Mock<IClientController>();
            _sut = new SessionHandler(_mockedClientController.Object);
            _packetDTO = new PacketDTO();
        }

        [Test]
        public void Test_JoinSession_InvalidSessionId()
        {
            // Arrange ------------
            string invalidSessionId = "invalid";

            using (StringWriter sw = new StringWriter())
            {
                //Act ---------
                Console.SetOut(sw);
                _sut.JoinSession(invalidSessionId);

                //Assert ---------
                string expected = string.Format("Could not find game!\r\n", Environment.NewLine);
                Assert.AreEqual(expected, sw.ToString());
            }
        }

        [Test]
        public void Test_JoinSession_ValidSessionId()
        {
            // Arrange ------------
            string sessionId = "sessionId";
            string hostOriginId = "hostTestOriginId";
            string originId = "testOriginId";

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessions);
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = hostOriginId;
            packetHeaderDTO.SessionID = sessionId;
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = originId;
            _packetDTO.Header = packetHeaderDTO;

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns(originId);

            SessionDTO sessionDTOInHandlerResponse = new SessionDTO(SessionType.RequestSessionsResponse);
            sessionDTOInHandlerResponse.Name = "sessionName";
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, JsonConvert.SerializeObject(sessionDTOInHandlerResponse));
            _packetDTO.HandlerResponse = handlerResponseDTO;

            _sut.HandlePacket(_packetDTO);

            SessionDTO expectedSessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
            expectedSessionDTO.ClientIds = new List<string>();
            expectedSessionDTO.ClientIds.Add(originId);
            var expectedPayload = JsonConvert.SerializeObject(expectedSessionDTO);

            _mockedClientController.Setup(mock => mock.SendPayload(expectedPayload, PacketType.Session));
            _mockedClientController.Setup(mock => mock.SetSessionId(sessionId));
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns(originId);

            //Act ---------
            _sut.JoinSession(sessionId);

            //Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(expectedPayload, PacketType.Session), Times.Once);
            _mockedClientController.Verify(mock => mock.SetSessionId(sessionId), Times.Once);
            _mockedClientController.Verify(mock => mock.GetOriginId(), Times.Exactly(2));
        }

        [Test]
        public void Test_CreateSession()
        {
            // Arrange ------------
            string testSessionName = "testSessionName";

            _mockedClientController.Setup(mock => mock.CreateHostController());
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()));

            // Act ----------------
            _sut.CreateSession(testSessionName);

            // Assert -------------
            _mockedClientController.Verify(mock => mock.CreateHostController(), Times.Once());
            _mockedClientController.Verify(mock => mock.SetSessionId(It.IsAny<string>()));
        }

        [Test]
        public void Test_RequestSessions()
        {
            // Arrange ---------
            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessions);
            var payload = JsonConvert.SerializeObject(sessionDTO);

            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Session));

            // Act ---------
            _sut.RequestSessions();

            // Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.Session), Times.Once());
        }

        [Test]
        public void Test_HandlePacket_RequestSessionsAtClientOrHost()
        {
            // Arrange ---------
            _sut.CreateSession("testSessionName");

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
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            HandlerResponseDTO expectedResult = new HandlerResponseDTO(SendAction.ReturnToSender, "testSessionName");
            Assert.AreEqual(expectedResult.Action, actualResult.Action);
        }

        [Test]
        public void Test_HandlePacket_RequestSessionsAtTargetedId()
        {
            // Arrange ---------
            string hostOriginId = "hostTestOriginId";
            string originId = "testOriginId";

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessions);
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = hostOriginId;
            packetHeaderDTO.SessionID = "sessionId";
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = originId;
            _packetDTO.Header = packetHeaderDTO;

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns(originId);

            SessionDTO sessionDTOInHandlerResponse = new SessionDTO(SessionType.RequestSessionsResponse);
            sessionDTOInHandlerResponse.Name = "sessionName";
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, JsonConvert.SerializeObject(sessionDTOInHandlerResponse));
            _packetDTO.HandlerResponse = handlerResponseDTO;

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            HandlerResponseDTO expectedResult = new HandlerResponseDTO(SendAction.Ignore, null);
            Assert.AreEqual(expectedResult.Action, actualResult.Action);
        }

        [Test]
        public void Test_HandlePacket_NotRequestSessionsTypeAtTargetedId()
        {
            // Arrange ---------
            string hostOriginId = "hostTestOriginId";
            string originId = "testOriginId";

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = hostOriginId;
            packetHeaderDTO.SessionID = "sessionId";
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = originId;
            _packetDTO.Header = packetHeaderDTO;

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns(originId);

            SessionDTO sessionDTOInHandlerResponse = new SessionDTO(SessionType.RequestSessionsResponse);
            sessionDTOInHandlerResponse.Name = "sessionName";
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, JsonConvert.SerializeObject(sessionDTOInHandlerResponse));
            _packetDTO.HandlerResponse = handlerResponseDTO;

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            HandlerResponseDTO expectedResult = new HandlerResponseDTO(SendAction.Ignore, null);
            Assert.AreEqual(expectedResult.Action, actualResult.Action);
        }

        [Test]
        public void Test_HandlePacket_AtNonTargetedId()
        {
            // Arrange ---------
            string hostOriginId = "hostTestOriginId";
            string originId = "testOriginId";

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = hostOriginId;
            packetHeaderDTO.SessionID = "sessionId";
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = "otherOriginId";
            _packetDTO.Header = packetHeaderDTO;

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns(originId);

            SessionDTO sessionDTOInHandlerResponse = new SessionDTO(SessionType.RequestSessionsResponse);
            sessionDTOInHandlerResponse.Name = "sessionName";
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, JsonConvert.SerializeObject(sessionDTOInHandlerResponse));
            _packetDTO.HandlerResponse = handlerResponseDTO;

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            HandlerResponseDTO expectedResult = new HandlerResponseDTO(SendAction.Ignore, null);
            Assert.AreEqual(expectedResult.Action, actualResult.Action);
        }

        [Test]
        public void Test_HandlePacket_RequestToJoinSessionThatDoesNotExist()
        {
            // Arrange ---------
            _sut.CreateSession("testSessionName");

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = "testOriginId";
            packetHeaderDTO.SessionID = "otherSessionId";
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            HandlerResponseDTO expectedResult = new HandlerResponseDTO(SendAction.Ignore, null);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Test_HandlePacket_RequestToJoinSessionThatExistsAtHost()
        {
            // Arrange ---------
            string generatedSessionId = "";
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>())).Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName");

            string originId = "testOriginId";

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
            sessionDTO.ClientIds = new List<string>();
            sessionDTO.ClientIds.Add(originId);

            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = originId;
            packetHeaderDTO.SessionID = generatedSessionId;
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;
            

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            SendAction expectedSendAction = SendAction.SendToClients;
            Assert.AreEqual(expectedSendAction, actualResult.Action);
        }

        [Test]
        public void Test_HandlePacket_RequestToJoinSessionThatExistsAtClient()
        {
            // Arrange ---------
            string generatedSessionId = "";
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>())).Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName");

            string originId = "testOriginId";
            string originIdHost = "testOriginIdHost";

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
            sessionDTO.ClientIds = new List<string>();
            sessionDTO.ClientIds.Add(originId);

            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.OriginID = originId;
            packetHeaderDTO.SessionID = generatedSessionId;
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = "client";
            _packetDTO.Header = packetHeaderDTO;

            SessionDTO sessionDTOInHandlerResponse = new SessionDTO(SessionType.RequestToJoinSession);
            sessionDTOInHandlerResponse.ClientIds = sessionDTO.ClientIds;
            sessionDTOInHandlerResponse.ClientIds.Add(originIdHost);
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, JsonConvert.SerializeObject(sessionDTOInHandlerResponse));
            _packetDTO.HandlerResponse = handlerResponseDTO;


            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            HandlerResponseDTO expectedResult = new HandlerResponseDTO(SendAction.Ignore, null);
            Assert.AreEqual(expectedResult, actualResult);
        }
        
        [Test]
        public void Test_HandlePacket_RequestHeartbeatReturnsCatch()
        {
            // Arrange ---------
            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestHeartbeat);
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            HandlerResponseDTO expectedResult = new HandlerResponseDTO(SendAction.Catch, "heartbeat");
            Assert.AreEqual(expectedResult.Action, actualResult.Action);
        }
    }
}