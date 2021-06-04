using Messages;
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
using System.Threading;
using UserInterface;
using Timer = System.Timers.Timer;

namespace Session.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class SessionHandlerTests
    {
        //Declaration and initialisation of constant variables
        private SessionHandler _sut;

        private PacketDTO _packetDTO;
        private const int HOSTINACTIVECOUNTER = 5;

        //Declaration of mocks
        private Mock<IClientController> _mockedClientController;
        private Mock<IMessageService> _mockedMessageService;
        private Mock<Session> _mockedSession;
        private Mock<IScreenHandler> _mockedScreenHandler;

        [SetUp]
        public void Setup()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            _mockedClientController = new();
            _mockedMessageService = new();
            _mockedScreenHandler = new();
            _sut = new SessionHandler(_mockedClientController.Object, _mockedScreenHandler.Object, _mockedMessageService.Object);
            _mockedSession = new Mock<Session>();
            _packetDTO = new PacketDTO();
        }

        [Test]
        public void Test_JoinSession_InvalidSessionId()
        {
            // Arrange ------------
            string invalidSessionId = "invalid";
            string userName = "Gerrit";

            using (StringWriter sw = new StringWriter()) {
                //Act ---------
                Console.SetOut(sw);
                _sut.JoinSession(invalidSessionId, userName);

            //Assert ---------
            string expected = "Could not find game!";
            _mockedMessageService.Verify(mock => mock.AddMessage(expected), Times.Once);
        }

        [Test]
        public void Test_JoinSession_ValidSessionId()
        {
            // Arrange ------------
            string sessionId = "sessionId";
            string hostOriginId = "hostTestOriginId";
            string originId = "testOriginId";
            string userName = "Gerrit";

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
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients,
                JsonConvert.SerializeObject(sessionDTOInHandlerResponse));
            _packetDTO.HandlerResponse = handlerResponseDTO;

            _sut.HandlePacket(_packetDTO);

            SessionDTO expectedSessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
            expectedSessionDTO.Clients = new List<string[]>();
            expectedSessionDTO.Clients.Add(new []{originId, userName});
            var expectedPayload = JsonConvert.SerializeObject(expectedSessionDTO);

            _mockedClientController.Setup(mock => mock.SendPayload(expectedPayload, PacketType.Session));
            _mockedClientController.Setup(mock => mock.SetSessionId(sessionId));
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns(originId);

            //Act ---------
            _sut.JoinSession(sessionId, userName);

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
            string hostName = "testHostName";

            _mockedClientController.Setup(mock => mock.CreateHostController());
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()));

            // Act ----------------
            _sut.CreateSession(testSessionName, hostName);

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


        [Test, Sequential]
        public void Test_HostPingEvent_SendPingPongReturnedCheck([Range(1, HOSTINACTIVECOUNTER)] int times)
        {
            // Arrange ---------
            SessionDTO sessionDTO = new SessionDTO(SessionType.SendPing);
            sessionDTO.Name = "ping";
            var payload = JsonConvert.SerializeObject(sessionDTO);

            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Session));

            _sut.setHostPingTimer(new Timer());

            // Act ---------
            for (int i = 0; i < times; i++)
            {
                Thread threadSut = new Thread(() => _sut.HostPingEvent(null, null));
                Thread threadHost = new Thread(() => _sut.setHostActive(true));

                threadSut.Start();

                //wait till other thread is sleeping to mock host pong
                var loop = true;
                while (loop)
                {
                    if (threadSut.ThreadState == ThreadState.WaitSleepJoin)
                    {
                        threadHost.Start();
                        loop = false;
                    }
                }

                threadHost.Join();
                threadSut.Join();
            }

            // Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.Session), Times.Exactly(times));
            _mockedClientController.Verify(mock => mock.CreateHostController(), Times.Never);
            _mockedClientController.Verify(mock => mock.IsBackupHost, Times.Never);
            Assert.IsTrue(_sut.getHostActive());
        }

        [Test, Sequential]
        public void Test_HostPingEvent_SendPingNoPongReturnedCheckUnderCount([Range(1, HOSTINACTIVECOUNTER - 1)] int times)
        {
            // Arrange ---------
            SessionDTO sessionDTO = new SessionDTO(SessionType.SendPing);
            sessionDTO.Name = "ping";
            var payload = JsonConvert.SerializeObject(sessionDTO);

            _mockedClientController.Setup(mock => mock.SendPayload(payload, PacketType.Session));

            _sut.setHostPingTimer(new Timer());

            //Act ---------
            for (int i = 0; i < times; i++)
            {
                _sut.HostPingEvent(null, null);
            }

            // Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payload, PacketType.Session), Times.Exactly(times));
            _mockedClientController.Verify(mock => mock.CreateHostController(), Times.Never);
            Assert.IsFalse(_sut.getHostActive());
        }

        [Test]
        public void Test_HostPingEvent_SendPingNoPongReturnedCheckCountHit()
        {
            // Arrange ---------
            string sessionId = "sessionId";
            string hostOriginId = "hostTestOriginId";
            string originId = "testOriginId";
            string userName = "Gerrit";

            SessionDTO sessionDTOjoin = new SessionDTO(SessionType.RequestSessions);
            var payload = JsonConvert.SerializeObject(sessionDTOjoin);
            _packetDTO.Payload = payload;
            Network.PacketHeaderDTO packetHeaderDTO = new Network.PacketHeaderDTO();
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

            SessionDTO sessionDTO = new SessionDTO(SessionType.SendPing);
            sessionDTO.Name = "ping";
            var payloadping = JsonConvert.SerializeObject(sessionDTO);

            _mockedClientController.Setup(mock => mock.SendPayload(payloadping, PacketType.Session));
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns("1");
            _sut.JoinSession(sessionId, userName);
            
            _sut.setHostPingTimer(new Timer());

            //Act ---------
            for (int i = 0; i < HOSTINACTIVECOUNTER; i++)
            {
                _sut.HostPingEvent(null, null);
            }

            // Assert ---------
            _mockedClientController.Verify(mock => mock.SendPayload(payloadping, PacketType.Session), Times.Exactly(HOSTINACTIVECOUNTER));
            _mockedClientController.Verify(mock => mock.CreateHostController(), Times.Once);
            Assert.IsTrue(_sut.getHostActive());
        }

        [Test]
        public void Test_HandlePacket_RequestSessionsAtClientOrHost()
        {
            // Arrange ---------
            _sut.CreateSession("testSessionName", "testHost");

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessions);
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            Network.PacketHeaderDTO packetHeaderDTO = new Network.PacketHeaderDTO();
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
            Network.PacketHeaderDTO packetHeaderDTO = new Network.PacketHeaderDTO();
            packetHeaderDTO.OriginID = hostOriginId;
            packetHeaderDTO.SessionID = "sessionId";
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = originId;
            _packetDTO.Header = packetHeaderDTO;

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns(originId);

            SessionDTO sessionDTOInHandlerResponse = new SessionDTO(SessionType.RequestSessionsResponse);
            sessionDTOInHandlerResponse.Name = "sessionName";
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients,
                JsonConvert.SerializeObject(sessionDTOInHandlerResponse));
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
            Network.PacketHeaderDTO packetHeaderDTO = new Network.PacketHeaderDTO();
            packetHeaderDTO.OriginID = hostOriginId;
            packetHeaderDTO.SessionID = "sessionId";
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = originId;
            _packetDTO.Header = packetHeaderDTO;

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns(originId);

            SessionDTO sessionDTOInHandlerResponse = new SessionDTO(SessionType.RequestSessionsResponse);
            sessionDTOInHandlerResponse.Name = "sessionName";
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients,
                JsonConvert.SerializeObject(sessionDTOInHandlerResponse));
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
            Network.PacketHeaderDTO packetHeaderDTO = new Network.PacketHeaderDTO();
            packetHeaderDTO.OriginID = hostOriginId;
            packetHeaderDTO.SessionID = "sessionId";
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = "otherOriginId";
            _packetDTO.Header = packetHeaderDTO;

            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns(originId);

            SessionDTO sessionDTOInHandlerResponse = new SessionDTO(SessionType.RequestSessionsResponse);
            sessionDTOInHandlerResponse.Name = "sessionName";
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients,
                JsonConvert.SerializeObject(sessionDTOInHandlerResponse));
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
            _sut.CreateSession("testSessionName", "testHost");

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            Network.PacketHeaderDTO packetHeaderDTO = new Network.PacketHeaderDTO();
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
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()))
                .Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName", "testHost");

            string originId = "testOriginId";

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
            sessionDTO.Clients = new List<string[]>();
            sessionDTO.Clients.Add(new []{originId, "Gerrit"});

            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            Network.PacketHeaderDTO packetHeaderDTO = new Network.PacketHeaderDTO();
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
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()))
                .Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName", "testHost");

            string originId = "testOriginId";
            string originIdHost = "testOriginIdHost";
            string userName = "Gerrit";

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
            sessionDTO.Clients = new List<string[]>();
            sessionDTO.Clients.Add(new []{ originId, userName });

            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            Network.PacketHeaderDTO packetHeaderDTO = new Network.PacketHeaderDTO();
            packetHeaderDTO.OriginID = originId;
            packetHeaderDTO.SessionID = generatedSessionId;
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = "client";
            _packetDTO.Header = packetHeaderDTO;

            SessionDTO sessionDTOInHandlerResponse = new SessionDTO(SessionType.RequestToJoinSession);
            sessionDTOInHandlerResponse.Clients = sessionDTO.Clients;
            sessionDTOInHandlerResponse.Clients.Add(new []{ originIdHost, userName });
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients,
                JsonConvert.SerializeObject(sessionDTOInHandlerResponse));
            _packetDTO.HandlerResponse = handlerResponseDTO;


            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            HandlerResponseDTO expectedResult = new HandlerResponseDTO(SendAction.Ignore, null);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Test_HandlePacket_RequestToJoinSessionAsSecondClient()
        {
            // Arrange ---------
            string generatedSessionId = "";
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()))
                .Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName", "testHost");

            string originId = "testOriginId";
            string originIdHost = "testOriginIdHost";
            string userName = "Gerrit";

            SessionDTO sessionDTO = new SessionDTO
            {
                SessionType = SessionType.RequestToJoinSession,
                Clients = new List<string[]>()
            };
            sessionDTO.Clients.Add(new []{originId, userName});

            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;

            Network.PacketHeaderDTO packetHeaderDTO = new Network.PacketHeaderDTO
            {
                OriginID = originId,
                SessionID = generatedSessionId,
                PacketType = PacketType.Session,
                Target = "client"
            };
            _packetDTO.Header = packetHeaderDTO;

            SessionDTO sessionDTOInHandlerResponse = new SessionDTO
            {
                SessionType = SessionType.RequestToJoinSession,
                Clients = sessionDTO.Clients
            };
            sessionDTOInHandlerResponse.Clients.Add(new []{originIdHost, userName});
            
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients,
                JsonConvert.SerializeObject(sessionDTOInHandlerResponse));
            _packetDTO.HandlerResponse = handlerResponseDTO;

            _mockedClientController.SetupSequence(x => x.GetOriginId()).Returns(originIdHost);

            // Act -------------
            _sut.HandlePacket(_packetDTO);

            // Assert ----------
            _mockedClientController.Verify(mock => mock.IsBackupHost, Times.Once);
            Assert.IsTrue(_sut.getHostPingTimer().Enabled);
            Assert.IsTrue(_sut.getHostPingTimer().AutoReset);
            Assert.AreEqual(1000, _sut.getHostPingTimer().Interval);
        }

        [Test]
        public void Test_HandlePacket_HostHandlePing()
        {
            // Arrange ---------
            string generatedSessionId = "";
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()))
                .Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName", "testHost");

            string originId = "testOriginId";
            string originIdHost = "testOriginIdHost";

            SessionDTO sessionDTO = new SessionDTO
            {
                SessionType = SessionType.SendPing,
                Name = "ping"
            };

            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            Network.PacketHeaderDTO packetHeaderDTO = new Network.PacketHeaderDTO
            {
                OriginID = originId,
                SessionID = generatedSessionId,
                PacketType = PacketType.Session,
                Target = "host"
            };

            _packetDTO.Header = packetHeaderDTO;
            SessionDTO sessionDTOInHandlerResponse = new SessionDTO
            {
                SessionType = SessionType.ReceivedPingResponse,
                Name = "pong"
            };

            var jsonObject = JsonConvert.SerializeObject(sessionDTOInHandlerResponse);
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.ReturnToSender, jsonObject);

            _mockedClientController.SetupSequence(x => x.GetOriginId()).Returns(originIdHost);

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            Assert.AreEqual(handlerResponseDTO, actualResult);
        }

        [Test]
        public void Test_HandlePacket_BackuphostHandlePong()
        {
            // Arrange ---------
            string generatedSessionId = "";
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()))
                .Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName", "testHost");

            string originId = "testOriginId";
            string originIdHost = "testOriginIdHost";

            SessionDTO sessionDTO = new SessionDTO
            {
                SessionType = SessionType.SendPing,
                Name = "ping"
            };

            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO
            {
                OriginID = originId,
                SessionID = generatedSessionId,
                PacketType = PacketType.Session,
                Target = originId
            };
            _packetDTO.Header = packetHeaderDTO;

            HandlerResponseDTO expectedHandlerResponse = new HandlerResponseDTO(SendAction.Ignore, null);
            _packetDTO.HandlerResponse = expectedHandlerResponse;

            _mockedClientController.SetupSequence(x => x.GetOriginId()).Returns(originId);

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            Assert.AreEqual(expectedHandlerResponse.Action, actualResult.Action);
            Assert.AreEqual(expectedHandlerResponse.ResultMessage, actualResult.ResultMessage);
            Assert.IsTrue(_sut.getHostActive());
        }

        [Test]
        public void Test_HandlePacket_ClientHandlePong()
        {
            // Arrange ---------
            string generatedSessionId = "";
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()))
                .Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName", "testHost");

            string originId = "testOriginId";
            string originIdHost = "testOriginIdHost";

            SessionDTO sessionDTO = new SessionDTO
            {
                SessionType = SessionType.SendPing,
                Name = "ping"
            };

            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO
            {
                OriginID = originId,
                SessionID = generatedSessionId,
                PacketType = PacketType.Session,
                Target = "client"
            };
            _packetDTO.Header = packetHeaderDTO;

            HandlerResponseDTO expectedHandlerResponse = new HandlerResponseDTO(SendAction.Ignore, null);
            _packetDTO.HandlerResponse = expectedHandlerResponse;

            _mockedClientController.SetupSequence(x => x.GetOriginId()).Returns(originId);

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            Assert.AreEqual(expectedHandlerResponse.Action, actualResult.Action);
            Assert.AreEqual(expectedHandlerResponse.ResultMessage, actualResult.ResultMessage);
        }

        [Test]
        public void Test_HandlePacket_RequestHeartbeat_Returns_Catch()
        {
            // Arrange ---------
            SessionDTO sessionDTO = new SessionDTO(SessionType.SendHeartbeat);
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            Network.PacketHeaderDTO packetHeaderDTO = new Network.PacketHeaderDTO();
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            HandlerResponseDTO expectedResult = new HandlerResponseDTO(SendAction.Ignore, null);
            Assert.AreEqual(expectedResult.Action, actualResult.Action);
        }
    }
}