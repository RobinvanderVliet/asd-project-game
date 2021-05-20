using Network;
using Newtonsoft.Json;
using Session.DTO;
using System;
using System.Collections.Generic;
using System.Threading;
using Network.DTO;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Session
{
    public class SessionHandler : IPacketHandler, ISessionHandler
    {
        private IClientController _clientController;
        private Session _session;
        private Dictionary<string, PacketDTO> _availableSessions = new();
        private bool _hostActive = true;
        private Timer _hostPingTimer;
        private const int WAITTIMEPINGTIMER = 500;
        private const int INTERVALTIMEPINGTIMER = 1000;
        // private IBackupHostService _backupHostService;
        
        private IHeartbeatHandler _heartbeat;
        public SessionHandler(IClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Session);
            // _backupHostService = backupHostService;
        }

        public void JoinSession(string sessionId)
        {
            if (!_availableSessions.TryGetValue(sessionId, out PacketDTO packetDTO))
            {
                Console.WriteLine("Could not find game!");
            }
            else
            {
                System.Threading.Timer timer = new System.Threading.Timer((e) =>
                {
                    SendHeartbeat();
                }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
       
                SessionDTO receivedSessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packetDTO.HandlerResponse.ResultMessage);
                _session = new Session(receivedSessionDTO.Name);

                _session.SessionId = sessionId;
                _clientController.SetSessionId(sessionId);
                Console.WriteLine("Trying to join game with name: " + _session.Name);

                SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
                sessionDTO.ClientIds = new List<string>();
                sessionDTO.ClientIds.Add(_clientController.GetOriginId());
                sendSessionDTO(sessionDTO);
            }
        }

        public void CreateSession(string sessionName)
        {
            _session = new Session(sessionName);
            _session.GenerateSessionId();
            _session.AddClient(_clientController.GetOriginId());
            _clientController.CreateHostController();
            _clientController.SetSessionId(_session.SessionId);
            _heartbeat = new HeartbeatHandler();
            Console.Out.WriteLine("Created session with the name: " + _session.Name);
        }

        public void RequestSessions()
        {
            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessions);
            sendSessionDTO(sessionDTO);
        }
        
        public void SendHeartbeat()
        {
            SessionDTO sessionDTO = new SessionDTO(SessionType.SendHeartbeat);
            sessionDTO.ClientIds = new List<string>();
            sessionDTO.ClientIds.Add(_clientController.GetOriginId());

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
            if (packet.Header.Target == "client" || packet.Header.Target == "host")
            {
                switch (sessionDTO.SessionType)
                {
                    case SessionType.SendHeartbeat:
                        return HandleHeartbeat();
                    case SessionType.RequestSessions:
                        return handleRequestSessions();
                    case SessionType.RequestToJoinSession:
                        if (packet.Header.SessionID == _session?.SessionId)
                        {
                            return addPlayerToSession(packet);
                        }
                        else
                        {
                            return new HandlerResponseDTO(SendAction.Ignore, null);
                        }
                    case SessionType.SendPing:
                        return handlePingRequest(packet);
                }
            }
            else if (packet.Header.Target == _clientController.GetOriginId())
            {
                if (sessionDTO.SessionType == SessionType.RequestSessions)
                {
                    return addRequestedSessions(packet);
                }
                else if (sessionDTO.SessionType == SessionType.SendPing) {
                    return handlePingRequest(packet);
                }

                return new HandlerResponseDTO(SendAction.Ignore, null);
            }
            
            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        private HandlerResponseDTO HandleHeartbeat()
        {
            if(_heartbeat != null)
            {
                _heartbeat.ReceiveHeartbeat(_clientController.GetOriginId());
            }

            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        private void CheckIfHostActive() 
        {
            if (!_hostActive) 
            {
                _hostPingTimer.Dispose();
                _hostActive = true;
                SwapToHost();
            }
        }
        
        private HandlerResponseDTO handlePingRequest(PacketDTO packet)
        {
            if (packet.Header.Target.Equals("client")) {
                return new HandlerResponseDTO(SendAction.Ignore, null);
            }
            if (packet.HandlerResponse != null)
            {
                Console.WriteLine("pong"); //TODO verwijderen
                _hostActive = true;
                return new HandlerResponseDTO(SendAction.Ignore, null);
            }
            else {
                SessionDTO sessionDTO = new SessionDTO {
                    SessionType = SessionType.ReceivedPingResponse,
                    Name = "pong"
                };
                var jsonObject = JsonConvert.SerializeObject(sessionDTO);
                return new HandlerResponseDTO(SendAction.ReturnToSender, jsonObject);
            }
        }

        private HandlerResponseDTO handleRequestSessions()
        {
            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessionsResponse);
            sessionDTO.Name = _session.Name;
            var jsonObject = JsonConvert.SerializeObject(sessionDTO);
            return new HandlerResponseDTO(SendAction.ReturnToSender, jsonObject);
        }

        private HandlerResponseDTO addRequestedSessions(PacketDTO packet)
        {
            _availableSessions.TryAdd(packet.Header.SessionID, packet);
            SessionDTO sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packet.HandlerResponse.ResultMessage);
            Console.WriteLine(packet.Header.SessionID + " Name: " + sessionDTO.Name);
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

                Console.Out.WriteLine("Players in your session:");
                foreach (string client in sessionDTOClients.ClientIds)
                {
                    _session.AddClient(client);
                    Console.Out.WriteLine(client);
                }
                
                if (sessionDTOClients.ClientIds.Count > 0 && !_clientController.IsBackupHost) {
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

        private void SendPing()
        {
            Console.WriteLine("ping"); //TODO verwijderen
            SessionDTO sessionDTO = new SessionDTO{
                SessionType = SessionType.SendPing,
                Name = "ping"
            };
            var jsonObject = JsonConvert.SerializeObject(sessionDTO);
            _hostActive = false;
            _clientController.SendPayload(jsonObject, PacketType.Session);
        }

        private void PingHostTimer()
        {
            _hostPingTimer = new System.Timers.Timer(INTERVALTIMEPINGTIMER);
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
            // TODO: Enable Heartbeat check and enable agents, maybe this will be done when hostcontroller is activated?
            // TODO: Make new client backup host
            
            Console.Out.WriteLine("Look at me, I'm the captain (Host) now!");
        }

        public Timer getHostPingTimer()
        {
            return _hostPingTimer;
        }

        public bool getHostActive()
        {
            return _hostActive;
        }
        
        public void setHostActive(Boolean boolean)
        {
            _hostActive = boolean;
        }

        public void setHostPingTimer(Timer timer)
        {
            _hostPingTimer = timer;
        }
    }
}
