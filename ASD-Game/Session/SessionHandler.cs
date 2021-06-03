using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using WorldGeneration;
using UserInterface;
using Timer = System.Timers.Timer;
using Creature.Creature.NeuralNetworking.TrainingScenario;

namespace Session
{
    public class SessionHandler : IPacketHandler, ISessionHandler
    {
        private const bool DEBUG_INTERFACE = true; //TODO: remove when UI is complete, obviously

        private IClientController _clientController;
        private Session _session;
        private IHeartbeatHandler _heartbeatHandler;
        public TrainingScenario trainingScenario { get; set; } = new TrainingScenario();
        private Dictionary<string, PacketDTO> _availableSessions = new();
        private bool _hostActive = true;
        private int _hostInactiveCounter = 0;
        private Timer _hostPingTimer;
        private Timer _senderHeartbeatTimer;
        private const int WAITTIMEPINGTIMER = 500;
        private const int INTERVALTIMEPINGTIMER = 1000;
        private IScreenHandler _screenHandler;

        public SessionHandler(IClientController clientController, IScreenHandler screenHandler)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Session);
            _screenHandler = screenHandler;
        }

        public List<string> GetAllClients()
        {
            return _session.GetAllClients();
        }

        public bool JoinSession(string sessionId)
        {
            var joinSession = false;

            if (!_availableSessions.TryGetValue(sessionId, out PacketDTO packetDTO))
            {
                Console.WriteLine("Could not find game!");
            }
            else
            {
                SendHeartbeatTimer();

                SessionDTO receivedSessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packetDTO.HandlerResponse.ResultMessage);
                _session = new Session(receivedSessionDTO.Name);

                _session.SessionId = sessionId;
                _clientController.SetSessionId(sessionId);
                Console.WriteLine("Trying to join game with name: " + _session.Name);

                SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
                sessionDTO.ClientIds = new List<string>();
                sessionDTO.ClientIds.Add(_clientController.GetOriginId());
                sessionDTO.SessionSeed = receivedSessionDTO.SessionSeed;
                sendSessionDTO(sessionDTO);
                joinSession = true;
            }

            return joinSession;
        }

        private void SendHeartbeatTimer()
        {
            _senderHeartbeatTimer = new Timer(INTERVALTIMEPINGTIMER);
            _senderHeartbeatTimer.Enabled = true;
            _senderHeartbeatTimer.AutoReset = true;
            _senderHeartbeatTimer.Elapsed += SenderHeartbeatEvent;
            _senderHeartbeatTimer.Start();
        }

        private void SenderHeartbeatEvent(object sender, ElapsedEventArgs e)
        {
            SendHeartbeat();
        }

        public bool CreateSession(string sessionName)
        {
            _session = new Session(sessionName);
            _session.GenerateSessionId();
            _session.AddClient(_clientController.GetOriginId());
            _session.SessionSeed = new MapFactory().GenerateSeed();
            _clientController.CreateHostController();
            _clientController.SetSessionId(_session.SessionId);
            _session.InSession = true;
            Thread traingThread = new Thread(
            trainingScenario.StartTraining
            );
            traingThread.Start();

            _heartbeatHandler = new HeartbeatHandler();
            Console.WriteLine("Created session with the name: " + _session.Name);

            return _session.InSession;
        }

        public void RequestSessions()
        {
            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessions);
            sendSessionDTO(sessionDTO);
        }

        public void SendHeartbeat()
        {
            SessionDTO sessionDTO = new SessionDTO(SessionType.SendHeartbeat);
            sendSessionDTO(sessionDTO);
        }

        private void sendSessionDTO(SessionDTO sessionDTO)
        {
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _clientController.SendPayload(payload, PacketType.Session);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            SessionDTO sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packet.Payload);

            if (packet.Header.SessionID == _session?.SessionId)
            {
                if (packet.Header.Target == "client" || packet.Header.Target == "host")
                {
                    if (sessionDTO.SessionType == SessionType.RequestToJoinSession)
                    {
                        return addPlayerToSession(packet);
                    }
                    if (sessionDTO.SessionType == SessionType.SendHeartbeat)
                    {
                        return HandleHeartbeat(packet);
                    }
                }
                if ((packet.Header.Target == "client" || packet.Header.Target == "host" || packet.Header.Target == _clientController.GetOriginId())
                    && sessionDTO.SessionType == SessionType.SendPing)
                {
                    return handlePingRequest(packet);
                }
            }
            else
            {
                if ((packet.Header.Target == "client" || packet.Header.Target == "host")
                    && sessionDTO.SessionType == SessionType.RequestSessions)
                {
                    return handleRequestSessions();
                }
                if (packet.Header.Target == _clientController.GetOriginId()
                    && sessionDTO.SessionType == SessionType.RequestSessions)
                {
                    return addRequestedSessions(packet);
                }
            }

            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        private HandlerResponseDTO HandleHeartbeat(PacketDTO packet)
        {
            if (_heartbeatHandler != null)
            {
                _heartbeatHandler.ReceiveHeartbeat(packet.Header.OriginID);
            }

            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        private void CheckIfHostActive()
        {
            if (!_hostActive)
            {
                _hostInactiveCounter++;
                if (_hostInactiveCounter >= 5)
                {
                    _hostPingTimer.Dispose();
                    _hostActive = true;
                    _hostInactiveCounter = 0;
                    SwapToHost();
                }
            }
            else
            {
                _hostInactiveCounter = 0;
            }
        }

        private HandlerResponseDTO handlePingRequest(PacketDTO packet)
        {
            if (packet.Header.Target.Equals("client"))
            {
                return new HandlerResponseDTO(SendAction.Ignore, null);
            }

            if (packet.HandlerResponse != null)
            {
                _hostActive = true;
            }
            else
            {
                SessionDTO sessionDTO = new SessionDTO
                {
                    SessionType = SessionType.ReceivedPingResponse,
                    Name = "pong"
                };
                var jsonObject = JsonConvert.SerializeObject(sessionDTO);
                return new HandlerResponseDTO(SendAction.ReturnToSender, jsonObject);
            }

            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        private HandlerResponseDTO handleRequestSessions()
        {
            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessionsResponse);
            sessionDTO.Name = _session.Name;
            sessionDTO.SessionSeed = _session.SessionSeed;
            var jsonObject = JsonConvert.SerializeObject(sessionDTO);
            return new HandlerResponseDTO(SendAction.ReturnToSender, jsonObject);
        }

        private HandlerResponseDTO addRequestedSessions(PacketDTO packet)
        {
            _availableSessions.TryAdd(packet.Header.SessionID, packet);
            SessionDTO sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packet.HandlerResponse.ResultMessage);

            if (!DEBUG_INTERFACE) // Remove when UI is completed
            {
                if (_screenHandler.Screen is SessionScreen)
                {
                    SessionScreen screen = _screenHandler.Screen as SessionScreen;
                    screen.UpdateSessions(sessionDTO.Name, packet.Header.SessionID);
                }
            }
            else
            {
                Console.WriteLine(
                    packet.Header.SessionID + " Name: " + sessionDTO.Name + " Seed: " + sessionDTO.SessionSeed);
            }
            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        private HandlerResponseDTO addPlayerToSession(PacketDTO packet)
        {
            SessionDTO sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packet.Payload);

            if (packet.Header.Target == "host")
            {
                Console.WriteLine(sessionDTO.ClientIds[0] + " Has joined your session: ");
                _session.AddClient(sessionDTO.ClientIds[0]);
                sessionDTO.ClientIds = new List<string>();

                sessionDTO.SessionSeed = _session.SessionSeed;

                foreach (string client in _session.GetAllClients())
                {
                    sessionDTO.ClientIds.Add(client);
                }

                return new HandlerResponseDTO(SendAction.SendToClients, JsonConvert.SerializeObject(sessionDTO));
            }
            else
            {
                SessionDTO sessionDTOClients = JsonConvert.DeserializeObject<SessionDTO>(packet.HandlerResponse.ResultMessage);
                _session.EmptyClients();

                _session.SessionSeed = sessionDTOClients.SessionSeed;

                Console.WriteLine("Players in your session:");
                foreach (string client in sessionDTOClients.ClientIds)
                {
                    _session.AddClient(client);
                    Console.WriteLine(client);
                }

                if (sessionDTOClients.ClientIds.Count > 0 && !_clientController.IsBackupHost)
                {
                    if (sessionDTOClients.ClientIds[1].Equals(_clientController.GetOriginId()))
                    {
                        _clientController.IsBackupHost = true;
                        PingHostTimer();
                        Console.WriteLine("You have been marked as the backup host");
                    }
                }
                return new HandlerResponseDTO(SendAction.Ignore, null);
            }
        }

        public int GetSessionSeed()
        {
            return _session.SessionSeed;
        }

        private void SendPing()
        {
            SessionDTO sessionDTO = new SessionDTO
            {
                SessionType = SessionType.SendPing,
                Name = "ping"
            };
            var jsonObject = JsonConvert.SerializeObject(sessionDTO);
            _hostActive = false;
            _clientController.SendPayload(jsonObject, PacketType.Session);
        }

        private void PingHostTimer()
        {
            _hostPingTimer = new Timer(INTERVALTIMEPINGTIMER);
            _hostPingTimer.Enabled = true;
            _hostPingTimer.AutoReset = true;
            _hostPingTimer.Elapsed += HostPingEvent;
            _hostPingTimer.Start();
        }

        public void HostPingEvent(Object source, ElapsedEventArgs e)
        {
            SendPing();
            Thread.Sleep(WAITTIMEPINGTIMER);
            CheckIfHostActive();
        }

        public void SwapToHost()
        {
            _clientController.CreateHostController();
            _clientController.IsBackupHost = false;

            _senderHeartbeatTimer.Close();

            Console.WriteLine("Look at me, I'm the captain (Host) now!");

            List<string> heartbeatSenders = new List<string>(_session.GetAllClients());
            heartbeatSenders.Remove(_clientController.GetOriginId());

            _heartbeatHandler = new HeartbeatHandler(heartbeatSenders);
        }

        public Timer getHostPingTimer()
        {
            return _hostPingTimer;
        }

        public bool getHostActive()
        {
            return _hostActive;
        }

        public void setHostActive(bool boolean)
        {
            _hostActive = boolean;
        }

        public void setHostPingTimer(Timer timer)
        {
            _hostPingTimer = timer;
        }
    }
}