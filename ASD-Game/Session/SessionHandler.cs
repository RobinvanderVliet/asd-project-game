using Network;
using Newtonsoft.Json;
using Session.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using Network.DTO;
using WorldGeneration;
using DatabaseHandler;
using DatabaseHandler.POCO;
using DatabaseHandler.Services;
using DatabaseHandler.Repository;
using Session.GameConfiguration;
using UserInterface;
using Timer = System.Timers.Timer;
using Messages;

namespace Session
{
    public class SessionHandler : IPacketHandler, ISessionHandler
    {
        private const bool DEBUG_INTERFACE = false; //TODO: remove when UI is complete, obviously

        private readonly IClientController _clientController;
        private Session _session;
        private IHeartbeatHandler _heartbeatHandler;
        private readonly IScreenHandler _screenHandler;
        private readonly IMessageService _messageService;

        private Dictionary<string, PacketDTO> _availableSessions = new();
        private bool _hostActive = true;
        private int _hostInactiveCounter = 0;
        private Timer _hostPingTimer;
        private Timer _senderHeartbeatTimer;
        private readonly IGameConfigurationHandler _gameConfigurationHandler;
        private const int WAITTIMEPINGTIMER = 500;
        private const int INTERVALTIMEPINGTIMER = 1000;
        public string GameName { get; set; }

        public SessionHandler(IClientController clientController, IScreenHandler screenHandler,
            IMessageService messageService, IGameConfigurationHandler gameConfigurationHandler)
        {
            _clientController = clientController;
            _screenHandler = screenHandler;
            _messageService = messageService;
            _gameConfigurationHandler = gameConfigurationHandler;
            _clientController.SubscribeToPacketType(this, PacketType.Session);
        }

        public SessionHandler(IMessageService messageService, IScreenHandler screenHandler,
            IGameConfigurationHandler gameConfigurationHandler)
        {
            _messageService = messageService;
            _screenHandler = screenHandler;
            _gameConfigurationHandler = gameConfigurationHandler;
            _messageService = messageService;
        }

        public SessionHandler()
        {
        }

        public List<string[]> GetAllClients()
        {
            return _session.GetAllClients();
        }

        public bool JoinSession(string sessionId, string userName)
        {
            var joinSession = false;

            if (!_availableSessions.TryGetValue(sessionId, out PacketDTO packetDTO))
            {
                _messageService.AddMessage("Could not find game!");
            }
            else
            {
                SendHeartbeatTimer();

                SessionDTO receivedSessionDTO =
                    JsonConvert.DeserializeObject<SessionDTO>(packetDTO.HandlerResponse.ResultMessage);
                _session = new Session(receivedSessionDTO.Name);

                _session.SessionId = sessionId;
                _clientController.SetSessionId(sessionId);
                _messageService.AddMessage("Trying to join game with name: " + _session.Name);

                SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
                sessionDTO.Clients = new List<string[]>();
                sessionDTO.Clients.Add(new[] {_clientController.GetOriginId(), userName});
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

        public bool CreateSession(string sessionName, string userName, bool savedGame, string sessionId, int? seed)
        {
            GameName = sessionName;
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

            _session.AddClient(_clientController.GetOriginId(), userName);

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
            _messageService.AddMessage("Created session with the name: " + _session.Name);
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


        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            SessionDTO sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packet.Payload);

            if (packet.Header.SessionID == _session?.SessionId)
            {
                if (packet.Header.Target == _clientController.GetOriginId() &&
                    sessionDTO.SessionType == SessionType.RequestToJoinSession)
                {
                    JoinExistingGame(packet);
                }

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


                    if (sessionDTO.SessionType == SessionType.NewBackUpHost)
                    {
                        return HandleNewBackupHost(packet);
                    }
                }

                if ((packet.Header.Target == "client" || packet.Header.Target == "host" ||
                     packet.Header.Target == _clientController.GetOriginId()))
                {
                    if (sessionDTO.SessionType == SessionType.EditMonsterDifficulty)
                    {
                        return HandleMonsterDifficulty(packet);
                    }

                    if (sessionDTO.SessionType == SessionType.EditItemSpawnRate)
                    {
                        return HandleItemSpawnRate(packet);
                    }
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

        private void JoinExistingGame(PacketDTO packet)
        {
            if (packet.HandlerResponse.ResultMessage != null)
            {
                if (packet.HandlerResponse.ResultMessage.Equals($"Not allowed to join saved or running game"))
                {
                    _messageService.AddMessage(packet.HandlerResponse.ResultMessage);
                }

                PacketDTO newPacket = new PacketBuilder().SetTarget("client").SetSessionID(_clientController.SessionId)
                    .SetPayload(packet.HandlerResponse.ResultMessage).SetPacketType(PacketType.GameSession).Build();
                ((IPacketHandler) _clientController).HandlePacket(newPacket);
            }
        }

        private HandlerResponseDTO HandleMonsterDifficulty(PacketDTO packetDto)
        {
            if (_clientController.IsHost())
            {
                SessionDTO sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packetDto.Payload);
                int difficulty = int.Parse(sessionDTO.Name);
                _gameConfigurationHandler.SetDifficulty((MonsterDifficulty) difficulty, _clientController.SessionId);
                return new HandlerResponseDTO(SendAction.SendToClients, packetDto.Payload);
            }

            if (_clientController.IsBackupHost)
            {
                SessionDTO sessionDTO =
                    JsonConvert.DeserializeObject<SessionDTO>(packetDto.HandlerResponse.ResultMessage);
                int difficulty = int.Parse(sessionDTO.Name);
                _gameConfigurationHandler.SetDifficulty((MonsterDifficulty) difficulty, _clientController.SessionId);
            }

            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        private HandlerResponseDTO HandleItemSpawnRate(PacketDTO packetDto)
        {
            if (_clientController.IsHost())
            {
                SessionDTO sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packetDto.Payload);
                int spawnrate = int.Parse(sessionDTO.Name);
                _messageService.AddMessage(spawnrate + "");
                _gameConfigurationHandler.SetSpawnRate((ItemSpawnRate) spawnrate, _clientController.SessionId);
                return new HandlerResponseDTO(SendAction.SendToClients, packetDto.Payload);
            }

            if (_clientController.IsBackupHost)
            {
                SessionDTO sessionDTO =
                    JsonConvert.DeserializeObject<SessionDTO>(packetDto.HandlerResponse.ResultMessage);
                int spawnrate = int.Parse(sessionDTO.Name);
                _messageService.AddMessage(spawnrate + "");
                _gameConfigurationHandler.SetSpawnRate((ItemSpawnRate) spawnrate, _clientController.SessionId);
            }

            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        public HandlerResponseDTO HandleNewBackupHost(PacketDTO packet)
        {
            if (packet.Header.Target == "host")
            {
                return new HandlerResponseDTO(SendAction.SendToClients, null);
            }
            else
            {
                bool nextBackupHost = GetAllClients().ElementAt(
                        GetAllClients().IndexOf(
                            GetAllClients().FirstOrDefault(i => i[0] == packet.Header.OriginID)) + 1)[0]
                    .Equals(_clientController.GetOriginId());

                if (!_clientController.IsBackupHost && nextBackupHost)
                {
                    //TODO reanable this after datatransfer is done
                    /*
                    _clientController.IsBackupHost = true;
                    PingHostTimer();
                    */
                    Console.WriteLine("I'm Mr. BackupHost! Look at me!");
                    return new HandlerResponseDTO(SendAction.Ignore, null);
                }

                return new HandlerResponseDTO(SendAction.Ignore, null);
            }
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
                if (_screenHandler.Screen is SessionScreen screen)
                {
                    var hostName = String.Empty;
                    var amountOfPlayers = "0";
                    if (sessionDTO.Clients != null && sessionDTO.Clients.Count > 0)
                    {
                        hostName = sessionDTO.Clients.First()[1];
                        amountOfPlayers = sessionDTO.Clients.Count.ToString();
                    }
                    else
                    {
                        // TODO: remove after/during integration
                        hostName = "Unnamed player";
                        amountOfPlayers = "1";
                    }

                    screen.UpdateWithNewSession(new[]
                        {packet.Header.SessionID, sessionDTO.Name, hostName, amountOfPlayers});
                }
            }
            else
            {
                _messageService.AddMessage("Id: " + packet.Header.SessionID + " Name: " + sessionDTO.Name + " Host: " +
                                           sessionDTO.Clients.First()[1] + " Amount of players: " +
                                           sessionDTO.Clients.Count);
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
                // Console.WriteLine(sessionDTO.Clients[0] + " has joined your session: ");
                _session.AddClient(sessionDTO.Clients[0][0], sessionDTO.Clients[0][1]);
                sessionDTO.Clients = new List<string[]>();
                if (_screenHandler.Screen is LobbyScreen screen)
                {
                    screen.UpdateLobbyScreen(sessionDTO.Clients);
                }

                sessionDTO.SessionSeed = _session.SessionSeed;

                foreach (string[] client in _session.GetAllClients())
                {
                    sessionDTO.Clients.Add(client);
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

            foreach (string[] client in sessionDTOClients.Clients)
            {
                _session.AddClient(client[0], client[1]);
            }

            if (_screenHandler.Screen is LobbyScreen screen)
            {
                screen.UpdateLobbyScreen(sessionDTOClients.Clients);
            }

            if (sessionDTOClients.Clients.Count > 0 && !_clientController.IsBackupHost)
            {
                if (sessionDTOClients.Clients[1][0].Equals(_clientController.GetOriginId()))
                {
                    _clientController.IsBackupHost = true;
                    PingHostTimer();
                }

                if (sessionDTOClients.Clients.Count > 0 && !_clientController.IsBackupHost)
                {
                    if (sessionDTOClients.Clients[1].Equals(_clientController.GetOriginId()))
                    {
                        _clientController.IsBackupHost = true;
                        PingHostTimer();
                        Console.WriteLine("You have been marked as the backup host");
                    }
                }
            }
        }

        private HandlerResponseDTO ActiveGameAddsPlayer(SessionDTO sessionDTO, PacketDTO packet)
        {
            // check if ID matches
            var clientId = sessionDTO.Clients[0];

            IDatabaseService<PlayerPOCO> servicePlayer = new DatabaseService<PlayerPOCO>();

            var allPlayerId = servicePlayer.GetAllAsync();
            allPlayerId.Wait();
            var result =
                allPlayerId.Result.FirstOrDefault(x =>
                    x.GameGuid == _session.SessionId && x.PlayerGuid == clientId[0]);

            if (result != null)
            {
                Console.WriteLine(sessionDTO.Clients[0] + " has joined your session: ");
                _session.AddClient(sessionDTO.Clients[0][0], sessionDTO.Clients[0][1]);
                sessionDTO.Clients = new List<string[]>();

                sessionDTO.SessionSeed = _session.SessionSeed;
                StartGameDTO startGameDto = null;

                if (GameStarted())
                {
                    startGameDto = HandlePlayerLocation(servicePlayer, result);
                }

                var jsonObject = JsonConvert.SerializeObject(startGameDto);

                return new HandlerResponseDTO(SendAction.ReturnToSender, jsonObject);
            }
            else
            {
                return new HandlerResponseDTO(SendAction.ReturnToSender,
                    "Not allowed to join saved or running game");
            }
        }

        public StartGameDTO HandlePlayerLocation(IDatabaseService<PlayerPOCO> servicePlayer, PlayerPOCO result)
        {
            StartGameDTO startGameDTO = new StartGameDTO();
            startGameDTO.ExistingPlayer = result;
            startGameDTO.Seed = _session.SessionSeed;

            var allPlayerId = servicePlayer.GetAllAsync();
            allPlayerId.Wait();
            var playerLocations = allPlayerId.Result.Where(x => x.GameGuid == _session.SessionId);

            startGameDTO.SavedPlayers = playerLocations.ToList();

            return startGameDTO;
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

            _messageService.AddMessage("Look at me, I'm the captain (Host) now!");

            var clients = _session.GetAllClients().Select(client => client.First()).ToArray();
            List<string> heartbeatSenders = new List<string>(clients);
            heartbeatSenders.Remove(_clientController.GetOriginId());

            _heartbeatHandler = new HeartbeatHandler(heartbeatSenders);

            SessionDTO sessionDTO = new SessionDTO
            {
                SessionType = SessionType.NewBackUpHost,
                Name = "you'are our co-captain (back up host) now!"
            };
            var jsonObject = JsonConvert.SerializeObject(sessionDTO);
            _clientController.SendPayload(jsonObject, PacketType.Session);
        }

        public Timer getHostPingTimer()
        {
            return _hostPingTimer;
        }

        public bool getHostActive()
        {
            return _hostActive;
        }

        public void setHostActive(bool hostActive)
        {
            _hostActive = hostActive;
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

        public void SetGameStarted(bool startSession)
        {
            _session.GameStarted = startSession;
        }


        public void SetSession(Session ses)
        {
            _session = ses;
        }
    }
}