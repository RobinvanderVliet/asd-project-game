using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.Session;
using ASD_Game.Session.DTO;
using ASD_Game.Session.GameConfiguration;
using ASD_Game.UserInterface;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Timer = System.Timers.Timer;

namespace ASD_Game.Tests.SessionTests
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
        private Mock<Session.Session> _mockedSession;
        private Mock<IScreenHandler> _mockedScreenHandler;
        private Mock<IGameConfigurationHandler> _mockedGameConfigurationHandler;

        [SetUp]
        public void Setup()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            _mockedClientController = new Mock<IClientController>();
            _mockedScreenHandler = new Mock<IScreenHandler>();
            _mockedGameConfigurationHandler = new Mock<IGameConfigurationHandler>();
            _mockedMessageService = new();
            _mockedScreenHandler = new();
            _sut = new SessionHandler(_mockedClientController.Object, _mockedScreenHandler.Object, _mockedGameConfigurationHandler.Object, _mockedMessageService.Object);
            _mockedSession = new Mock<Session.Session>("test");
            _packetDTO = new PacketDTO();
        }

        [Test]
        public void Test_JoinSession_InvalidSessionId()
        {
            // Arrange ------------
            string invalidSessionId = "invalid";
            string userName = "Gerrit";

            //Act ---------
            _sut.JoinSession(invalidSessionId, "");

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
            expectedSessionDTO.Clients.Add(new string[]{ originId, ""});
            var expectedPayload = JsonConvert.SerializeObject(expectedSessionDTO);

            _mockedClientController.Setup(mock => mock.SendPayload(expectedPayload, PacketType.Session));
            _mockedClientController.Setup(mock => mock.SetSessionId(sessionId));
            _mockedClientController.Setup(mock => mock.GetOriginId()).Returns(originId);

            //Act ---------
            _sut.JoinSession(sessionId, "");

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
            _sut.CreateSession(testSessionName, "");

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
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
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
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
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
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()))
                .Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName", "testHost");

            string originId = "testOriginId";

            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
            sessionDTO.Clients = new List<string[]>();
            sessionDTO.Clients.Add(new []{originId, "Gerrit"});

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
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
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

            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO
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
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO
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
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
            packetHeaderDTO.PacketType = PacketType.Session;
            packetHeaderDTO.Target = "host";
            _packetDTO.Header = packetHeaderDTO;

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            HandlerResponseDTO expectedResult = new HandlerResponseDTO(SendAction.Ignore, null);
            Assert.AreEqual(expectedResult.Action, actualResult.Action);
        }
        
        [Test]
        public void Test_HandlePacket_MonsterDifficultyHost()
        {
            // Arrange ---------
            string generatedSessionId = "";
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()))
                .Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName", "testHost");
            _mockedClientController.Setup(mock => mock.SessionId).Returns(generatedSessionId);
            _mockedClientController.Setup(mock => mock.IsHost()).Returns(true);
            _mockedClientController.Setup(mock => mock.IsBackupHost).Returns(false);

            string originId = "testOriginId";
            string originIdHost = "testOriginIdHost";
            MonsterDifficulty difficulty = MonsterDifficulty.Easy;

            SessionDTO sessionDTO = new SessionDTO {
                SessionType = SessionType.EditMonsterDifficulty,
                Name = ((int) difficulty).ToString()
            };

            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO
            {
                OriginID = originId,
                SessionID = generatedSessionId,
                PacketType = PacketType.Session,
                Target = "host"
            };
            _packetDTO.Header = packetHeaderDTO;
            
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, payload);

            _mockedClientController.SetupSequence(x => x.GetOriginId()).Returns(originIdHost);
            _mockedGameConfigurationHandler.SetupSequence(x => x.SetDifficulty(It.IsAny<MonsterDifficulty>(), It.IsAny<string>()));

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            Assert.AreEqual(handlerResponseDTO.ResultMessage, actualResult.ResultMessage);
            Assert.AreEqual(handlerResponseDTO.Action, actualResult.Action);
            _mockedGameConfigurationHandler.Verify(x => x.SetDifficulty(difficulty, generatedSessionId), Times.Once());
        }
        
        [Test]
        public void Test_HandlePacket_MonsterDifficultyBackupHost()
        {
            // Arrange ---------
            string generatedSessionId = "";
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()))
                .Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName", "testHost");
            _mockedClientController.Setup(mock => mock.SessionId).Returns(generatedSessionId);
            _mockedClientController.Setup(mock => mock.IsHost()).Returns(false);
            _mockedClientController.Setup(mock => mock.IsBackupHost).Returns(true);
            
            string originId = "testOriginId";
            string originIdHost = "testOriginIdHost";
            MonsterDifficulty difficulty = MonsterDifficulty.Easy;

            SessionDTO sessionDTO = new SessionDTO {
                SessionType = SessionType.EditMonsterDifficulty,
                Name = ((int) difficulty).ToString()
            };

            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO
            {
                OriginID = originId,
                SessionID = generatedSessionId,
                PacketType = PacketType.Session,
                Target = "host"
            };
            _packetDTO.Header = packetHeaderDTO;
            
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, payload);
            _packetDTO.HandlerResponse = handlerResponseDTO;

            _mockedClientController.SetupSequence(x => x.GetOriginId()).Returns(originIdHost);
            _mockedGameConfigurationHandler.SetupSequence(x => x.SetDifficulty(It.IsAny<MonsterDifficulty>(), It.IsAny<string>()));

            HandlerResponseDTO expected = new HandlerResponseDTO(SendAction.Ignore, null);
            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------;
            Assert.AreEqual(expected.Action, actualResult.Action);
            Assert.AreEqual(expected.ResultMessage, actualResult.ResultMessage);
            _mockedGameConfigurationHandler.Verify(x => x.SetDifficulty(difficulty, generatedSessionId), Times.Once());
        }
        
        [Test]
        public void Test_HandlePacket_MonsterDifficultyClient()
        {
            // Arrange ---------
            string generatedSessionId = "";
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()))
                .Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName", "testHost");
            _mockedClientController.Setup(mock => mock.SessionId).Returns(generatedSessionId);
            _mockedClientController.Setup(mock => mock.IsHost()).Returns(false);
            _mockedClientController.Setup(mock => mock.IsBackupHost).Returns(false);
            
            string originId = "testOriginId";
            string originIdHost = "testOriginIdHost";
            MonsterDifficulty difficulty = MonsterDifficulty.Easy;
            
            SessionDTO sessionDTO = new SessionDTO {
                SessionType = SessionType.EditMonsterDifficulty,
                Name = ((int) difficulty).ToString()
            };
            
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO
            {
                OriginID = originId,
                SessionID = generatedSessionId,
                PacketType = PacketType.Session,
                Target = "host"
            };
            _packetDTO.Header = packetHeaderDTO;

            _mockedClientController.SetupSequence(x => x.GetOriginId()).Returns(originIdHost);
            _mockedGameConfigurationHandler.SetupSequence(x => x.SetDifficulty(It.IsAny<MonsterDifficulty>(), It.IsAny<string>()));

            HandlerResponseDTO expected = new HandlerResponseDTO(SendAction.Ignore, null);
            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------;
            Assert.AreEqual(expected.Action, actualResult.Action);
            Assert.AreEqual(expected.ResultMessage, actualResult.ResultMessage);
            _mockedGameConfigurationHandler.Verify(x => x.SetDifficulty(difficulty, generatedSessionId), Times.Never());
        }
        
        [Test]
        public void Test_HandlePacket_ItemSpawnRateHost()
        {
            // Arrange ---------
            string generatedSessionId = "";
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()))
                .Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName", "testHost");
            _mockedClientController.Setup(mock => mock.SessionId).Returns(generatedSessionId);
            _mockedClientController.Setup(mock => mock.IsHost()).Returns(true);
            _mockedClientController.Setup(mock => mock.IsBackupHost).Returns(false);

            string originId = "testOriginId";
            string originIdHost = "testOriginIdHost";
            ItemSpawnRate spawnRate = ItemSpawnRate.Low;

            SessionDTO sessionDTO = new SessionDTO {
                SessionType = SessionType.EditItemSpawnRate,
                Name = ((int) spawnRate).ToString()
            };

            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO
            {
                OriginID = originId,
                SessionID = generatedSessionId,
                PacketType = PacketType.Session,
                Target = "host"
            };
            _packetDTO.Header = packetHeaderDTO;
            
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, payload);

            _mockedClientController.SetupSequence(x => x.GetOriginId()).Returns(originIdHost);
            _mockedGameConfigurationHandler.SetupSequence(x => x.SetSpawnRate(It.IsAny<ItemSpawnRate>(), It.IsAny<string>()));

            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------
            Assert.AreEqual(handlerResponseDTO.ResultMessage, actualResult.ResultMessage);
            Assert.AreEqual(handlerResponseDTO.Action, actualResult.Action);
            _mockedGameConfigurationHandler.Verify(x => x.SetSpawnRate(spawnRate, generatedSessionId), Times.Once());
        }
        
        [Test]
        public void Test_HandlePacket_ItemSpawnRateBackupHost()
        {
            // Arrange ---------
            string generatedSessionId = "";
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()))
                .Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName", "testHost");
            _mockedClientController.Setup(mock => mock.SessionId).Returns(generatedSessionId);
            _mockedClientController.Setup(mock => mock.IsHost()).Returns(false);
            _mockedClientController.Setup(mock => mock.IsBackupHost).Returns(true);
            
            string originId = "testOriginId";
            string originIdHost = "testOriginIdHost";
            ItemSpawnRate spawnRate = ItemSpawnRate.Low;

            SessionDTO sessionDTO = new SessionDTO {
                SessionType = SessionType.EditItemSpawnRate,
                Name = ((int) spawnRate).ToString()
            };

            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO
            {
                OriginID = originId,
                SessionID = generatedSessionId,
                PacketType = PacketType.Session,
                Target = "host"
            };
            _packetDTO.Header = packetHeaderDTO;
            
            HandlerResponseDTO handlerResponseDTO = new HandlerResponseDTO(SendAction.SendToClients, payload);
            _packetDTO.HandlerResponse = handlerResponseDTO;

            _mockedClientController.SetupSequence(x => x.GetOriginId()).Returns(originIdHost);
            _mockedGameConfigurationHandler.SetupSequence(x => x.SetSpawnRate(It.IsAny<ItemSpawnRate>(), It.IsAny<string>()));

            HandlerResponseDTO expected = new HandlerResponseDTO(SendAction.Ignore, null);
            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------;
            Assert.AreEqual(expected.Action, actualResult.Action);
            Assert.AreEqual(expected.ResultMessage, actualResult.ResultMessage);
            _mockedGameConfigurationHandler.Verify(x => x.SetSpawnRate(spawnRate, generatedSessionId), Times.Once());
        }
        
        [Test]
        public void Test_HandlePacket_SpawnRateClient()
        {
            // Arrange ---------
            string generatedSessionId = "";
            _mockedClientController.Setup(mock => mock.SetSessionId(It.IsAny<string>()))
                .Callback<string>(r => generatedSessionId = r);
            _sut.CreateSession("testSessionName", "testHost");
            _mockedClientController.Setup(mock => mock.SessionId).Returns(generatedSessionId);
            _mockedClientController.Setup(mock => mock.IsHost()).Returns(false);
            _mockedClientController.Setup(mock => mock.IsBackupHost).Returns(false);
            
            string originId = "testOriginId";
            string originIdHost = "testOriginIdHost";
            ItemSpawnRate spawnRate = ItemSpawnRate.Low;
            
            SessionDTO sessionDTO = new SessionDTO {
                SessionType = SessionType.EditItemSpawnRate,
                Name = ((int) spawnRate).ToString()
            };
            
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _packetDTO.Payload = payload;
            PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO
            {
                OriginID = originId,
                SessionID = generatedSessionId,
                PacketType = PacketType.Session,
                Target = "host"
            };
            _packetDTO.Header = packetHeaderDTO;

            _mockedClientController.SetupSequence(x => x.GetOriginId()).Returns(originIdHost);
            _mockedGameConfigurationHandler.SetupSequence(x => x.SetSpawnRate(It.IsAny<ItemSpawnRate>(), It.IsAny<string>()));

            HandlerResponseDTO expected = new HandlerResponseDTO(SendAction.Ignore, null);
            // Act -------------
            HandlerResponseDTO actualResult = _sut.HandlePacket(_packetDTO);

            // Assert ----------;
            Assert.AreEqual(expected.Action, actualResult.Action);
            Assert.AreEqual(expected.ResultMessage, actualResult.ResultMessage);
            _mockedGameConfigurationHandler.Verify(x => x.SetSpawnRate(spawnRate, generatedSessionId), Times.Never());
        }

        [Test]
        public void Test_AddPlayerToSession_HostTestIfUpdateIsCalled() 
        {
            //Arrange
            //Arrange packet to be recieved
            PacketDTO packetDTO = new PacketDTO();
            PacketHeaderDTO header = new PacketHeaderDTO();
            header.Target = "host";
            packetDTO.Header = header;
            SessionDTO sessionDTO = new SessionDTO();
            sessionDTO.Clients = new List<string[]>();
            sessionDTO.Clients.Add(new string[] { "1234", "swankie" });
            packetDTO.Payload = JsonConvert.SerializeObject(sessionDTO);
            packetDTO.HandlerResponse = new HandlerResponseDTO(SendAction.Ignore, "");

            Session.Session session = new Session.Session("testsession");
            _sut.SetSession(session);

            //Arrange the mock for lobbyscreen
            Mock<LobbyScreen> lobbyMock = new Mock<LobbyScreen>();
            _mockedScreenHandler.Setup(mock => mock.Screen).Returns(lobbyMock.Object);
            lobbyMock.Setup(mock => mock.UpdateLobbyScreen(It.IsAny<List<string[]>>()));

            //Act
            _sut.addPlayerToSession(packetDTO);

            //Assert
            lobbyMock.Verify(mock => mock.UpdateLobbyScreen(It.IsAny<List<string[]>>()), Times.Once());
        }

        [Test]
        public void Test_AddPlayerToSession_ClientIfUpdateIsCalled()
        {
            //Arrange
            //Arrange packet to be recieved
            PacketDTO packetDTO = new PacketDTO();
            PacketHeaderDTO header = new PacketHeaderDTO();
            header.Target = "client";
            packetDTO.Header = header;
            SessionDTO packetSessionDTO = new SessionDTO();
            packetSessionDTO.Clients = new List<string[]>();
            packetSessionDTO.Clients.Add(new string[] { "1234", "swankie" });
            packetDTO.Payload = JsonConvert.SerializeObject(packetSessionDTO);

            //ArrangeHandlerRespone
            SessionDTO resultMessage = new SessionDTO();
            resultMessage.Clients = new List<string[]>();
            resultMessage.Clients.Add(new string[] { "1234", "swankie" });
            resultMessage.SessionSeed = 1;
            packetDTO.HandlerResponse = new HandlerResponseDTO(SendAction.Ignore, JsonConvert.SerializeObject(resultMessage));

            Session.Session session = new Session.Session("testsession");
            _sut.SetSession(session);

            //Arrange the mock for lobbyscreen
            Mock<LobbyScreen> lobbyMock = new Mock<LobbyScreen>();
            _mockedScreenHandler.Setup(mock => mock.Screen).Returns(lobbyMock.Object);
            lobbyMock.Setup(mock => mock.UpdateLobbyScreen(It.IsAny<List<string[]>>()));
            _mockedClientController.Setup(mock => mock.IsBackupHost).Returns(true);

            //Act
            _sut.addPlayerToSession(packetDTO);

            //Assert
            lobbyMock.Verify(mock => mock.UpdateLobbyScreen(It.IsAny<List<string[]>>()), Times.Once());
        }

        [Test]
        public void Test_HandleNewBackupHost_Host()
        {
            //Arrange
            PacketDTO packet = new()
            {
                Header = new PacketHeaderDTO() {Target = "host"},
                Payload = ""
            };

            //Act
            var result = _sut.HandleNewBackupHost(packet);
            
            //Assert
            Assert.AreEqual(SendAction.SendToClients, result.Action);
        }

        [Test]
        public void Test_HandleNewBackupHost_Client_Next()
        {
            //Arrange
            PacketDTO packet = new()
            {
                Header = new PacketHeaderDTO() { OriginID = "2", Target = "client" },
                Payload = ""
            };

            _sut.SetSession(new Session.Session("test game"));
            _mockedClientController.Setup(x => x.GetOriginId()).Returns("3");

            _sut.GetAllClients().Add(new []{"1", "gerrit"});
            _sut.GetAllClients().Add(new[]{"2","henk"});
            _sut.GetAllClients().Add(new[]{"3","jan"});

            //Act
            var result = _sut.HandleNewBackupHost(packet);

            //Assert
            Assert.AreEqual(result.Action, SendAction.Ignore);

            //Remove all clients for other test
            _sut.GetAllClients().RemoveRange(0, 3);
        }

        [Test]
        public void Test_HandleNewBackupHost_Client_NotNext()
        {
            //Arrange
            PacketDTO packet = new()
            {
                Header = new PacketHeaderDTO() {OriginID = "2", Target = "client" },
                Payload = ""
            };

            _sut.SetSession(new Session.Session("test game"));
            _mockedClientController.Setup(x => x.GetOriginId()).Returns("1");

            _sut.GetAllClients().Add(new []{"1", "gerrit"});
            _sut.GetAllClients().Add(new[]{"2","henk"});
            _sut.GetAllClients().Add(new[]{"3","jan"});

            //Act
            var result = _sut.HandleNewBackupHost(packet);

            //Assert
            Assert.AreEqual(result.Action, SendAction.Ignore);

            //Remove all clients for other test
            _sut.GetAllClients().RemoveRange(0, 3);
        }

        [Test]
        public void Test_HandlePacket_NewBackupHost()
        {
            //Arrange
            PacketDTO packet = new()
            {
                Header = new PacketHeaderDTO() { OriginID = "1", Target = "client" },
                Payload = JsonConvert.SerializeObject(new SessionDTO() {
                    SessionType = SessionType.NewBackUpHost,
                    SessionSeed = 0,
                    Clients = new List<String[]>(),
                    Name = ""
                })
            };

            //needed for list
            _sut.SetSession(_mockedSession.Object);
            _sut.GetAllClients().Add(new []{"1", "gerrit"});
            _sut.GetAllClients().Add(new[]{"2","henk"});
            _sut.GetAllClients().Add(new[]{"3","jan"});

            //Act
            var result = _sut.HandlePacket(packet);

            //Assert
            Assert.AreEqual(result.Action, SendAction.Ignore);
        }

    }
}