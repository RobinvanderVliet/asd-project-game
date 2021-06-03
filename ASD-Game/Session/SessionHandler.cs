using Network;
using Network.DTO;
using Newtonsoft.Json;
using Session.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Repository;
using DatabaseHandler.Services;
using Network.DTO;
using WorldGeneration;

using UserInterface;
using Timer = System.Timers.Timer;

namespace Session
{
    public class SessionHandler : IPacketHandler, ISessionHandler
    {
        private const bool DEBUG_INTERFACE = true; //TODO: remove when UI is complete, obviously
        
        private IClientController _clientController;
        private Session _session;
        private IHeartbeatHandler _heartbeatHandler;
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

                    SessionDTO receivedSessionDTO =
                        JsonConvert.DeserializeObject<SessionDTO>(packetDTO.HandlerResponse.ResultMessage);
                    _session = new Session(receivedSessionDTO.Name);

                    _session.SessionId = sessionId;
                    _clientController.SetSessionId(sessionId);
                    Console.WriteLine("Trying to join game with name: " + _session.Name);

                    SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
                    sessionDTO.ClientIds = new List<string>();
                    sessionDTO.ClientIds.Add(_clientController.GetOriginId());
                    sessionDTO.SessionSeed = receivedSessionDTO.SessionSeed;
                    SendSessionDTO(sessionDTO);
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


        public bool CreateSession(string sessionName, bool savedGame, string sessionId, int? seed)
        {
            _session = new Session(sessionName);
            _session.SessionId = sessionId;
            if (sessionId is null)
            {
                _session.GenerateSessionId();
            }
            else
            {
                _session.SessionId = sessionId;
            }

            _session.AddClient(_clientController.GetOriginId());

            if (seed != null)
            {
             _session.SessionSeed = seed.Value; 
               
            }
            else
            {
                _session.SessionSeed = MapFactory.GenerateSeed();
            }
            _clientController.CreateHostController();
            _clientController.SetSessionId(_session.SessionId);
            _session.InSession = true;
            _session.SavedGame = savedGame;

            _heartbeatHandler = new HeartbeatHandler();
            Console.WriteLine("Created session with the name: " + _session.Name);

            return _session.InSession;
        }

        public void RequestSessions()
        {
            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessions);
            SendSessionDTO(sessionDTO);
        }
        
        public void SendHeartbeat()
        {
            SessionDTO sessionDTO = new SessionDTO(SessionType.SendHeartbeat);
            SendSessionDTO(sessionDTO);
        }

        private void SendSessionDTO(SessionDTO sessionDTO)
        {
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _clientController.SendPayload(payload, PacketType.Session);
        }

        public void SendExistingPlayer(StartGameDTO startGameDTO)
        {
            var payload = JsonConvert.SerializeObject(startGameDTO);
            _clientController.SendPayload(payload, PacketType.GameSession);
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
                        return AddPlayerToSession(packet);
                    }
                    if (sessionDTO.SessionType == SessionType.SendHeartbeat)
                    {
                        return HandleHeartbeat(packet);
                    }
                }
                
                if (packet.Header.Target.Equals(_clientController.GetOriginId()))
                {
                    Console.WriteLine(packet.HandlerResponse.ResultMessage);
                }

                if ((packet.Header.Target == "client" || packet.Header.Target == "host" ||
                     packet.Header.Target == _clientController.GetOriginId())
                    && sessionDTO.SessionType == SessionType.SendPing)
                {
                    return HandlePingRequest(packet);
                }
            }
            else
            {
                if ((packet.Header.Target == "client" || packet.Header.Target == "host")
                    && sessionDTO.SessionType == SessionType.RequestSessions)
                {
                    return HandleRequestSessions();
                }

                if (packet.Header.Target == _clientController.GetOriginId()
                    && sessionDTO.SessionType == SessionType.RequestSessions)
                {
                    return AddRequestedSessions(packet);
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

        private HandlerResponseDTO HandlePingRequest(PacketDTO packet)
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

        private HandlerResponseDTO HandleRequestSessions()
        {
            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessionsResponse);
            sessionDTO.Name = _session.Name;
            sessionDTO.SessionSeed = _session.SessionSeed;
            sessionDTO.SavedGame = _session.SavedGame;
            var jsonObject = JsonConvert.SerializeObject(sessionDTO);
            return new HandlerResponseDTO(SendAction.ReturnToSender, jsonObject);
        }

        private HandlerResponseDTO AddRequestedSessions(PacketDTO packet)
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

    private HandlerResponseDTO AddPlayerToSession(PacketDTO packet)
        {
            SessionDTO sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packet.Payload);

            if (packet.Header.Target == "host")
            {
                return HostAddsPlayer(sessionDTO, packet);
            }
            else
            {
                ClientAddsPlayer(sessionDTO, packet);
            }

            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        private HandlerResponseDTO HostAddsPlayer(SessionDTO sessionDTO, PacketDTO packet)
        {
            if (_session.SavedGame || _session.GameStarted)
            {
                return ActiveGameAddsPlayer(sessionDTO, packet);
            }
            else
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
        }

        private void ClientAddsPlayer(SessionDTO sessionDTO, PacketDTO packet)
        {
            SessionDTO sessionDTOClients =
                    JsonConvert.DeserializeObject<SessionDTO>(packet.HandlerResponse.ResultMessage);
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
        }

        private HandlerResponseDTO ActiveGameAddsPlayer(SessionDTO sessionDTO, PacketDTO packet)
        {
            // check if id komt overeen
            var clientId = sessionDTO.ClientIds[0];

            IDatabaseService<PlayerPOCO> servicePlayer = new DatabaseService<PlayerPOCO>();

            var allPlayerId = servicePlayer.GetAllAsync();
            allPlayerId.Wait();
            var result =
                allPlayerId.Result.FirstOrDefault(x => x.GameGuid == _session.SessionId && x.PlayerGuid == clientId);

            if (result != null)
            {
                Console.WriteLine(sessionDTO.ClientIds[0] + " Has joined your session: ");
                _session.AddClient(sessionDTO.ClientIds[0]);
                sessionDTO.ClientIds = new List<string>();

                sessionDTO.SessionSeed = _session.SessionSeed;

                if (GameStarted())
                {
                    HandlePlayerLocation(servicePlayer, result);
                }
                return new HandlerResponseDTO(SendAction.Ignore, null);
            }
            else
            {
                return new HandlerResponseDTO(SendAction.ReturnToSender, "Not allowed to join saved or running game");
            }
            return new HandlerResponseDTO(SendAction.SendToClients, JsonConvert.SerializeObject(sessionDTO));
        }

        public void HandlePlayerLocation(IDatabaseService<PlayerPOCO> servicePlayer, PlayerPOCO result)
        {
            StartGameDTO joinedPlayerDto = new StartGameDTO();
            joinedPlayerDto.ExistingPlayer = result;
            joinedPlayerDto.Seed = _session.SessionSeed;

            var allPlayerId = servicePlayer.GetAllAsync();
            allPlayerId.Wait();
            var playerLocations = allPlayerId.Result.Where(x => x.GameGuid == _session.SessionId);
            Dictionary<string, int[]> players = new Dictionary<string, int[]>();

            foreach (var element in playerLocations)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = element.XPosition;
                playerPosition[1] = element.YPosition;
                players.Add(element.PlayerGuid, playerPosition);
            }

            joinedPlayerDto.PlayerLocations = players;
            SendExistingPlayer(joinedPlayerDto);
        }

        public int GetSessionSeed()
        {
            return _session.SessionSeed;
        }

        private void SendPing()
        {
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

        public bool GetSavedGame()
        {
            return _session.SavedGame;
        }

        public string GetSavedGameName()
        {
            return _session.Name;
        }

        public bool GameStarted()
        {
            return _session.GameStarted;
        }

        public void SetGameStarted(bool startSessie)
        {
            _session.GameStarted = startSessie;
        }
    }
}