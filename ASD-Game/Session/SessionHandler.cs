using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using ASD_Game.DatabaseHandler.POCO;
using ASD_Game.DatabaseHandler.Services;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.Session.DTO;
using ASD_Game.Session.Exceptions;
using ASD_Game.Session.GameConfiguration;
using ASD_Game.UserInterface;
using ASD_Game.World;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking.TrainingScenario;
using Newtonsoft.Json;
using WebSocketSharp;
using Timer = System.Timers.Timer;

namespace ASD_Game.Session
{
    public class SessionHandler : IPacketHandler, ISessionHandler
    {
        private const int WAITTIMEPINGTIMER = 500;
        private const int INTERVALTIMEPINGTIMER = 1000;
        private const int MAXPLAYERS = 8;

        private readonly IClientController _clientController;

        private Session _session;
        private IHeartbeatHandler _heartbeatHandler;
        public TrainingScenario TrainingScenario { get; set; } = new TrainingScenario();
        private readonly IScreenHandler _screenHandler;
        private readonly IMessageService _messageService;
        private readonly IDatabaseService<PlayerPOCO> _playerService;

        private Dictionary<string, PacketDTO> _availableSessions = new();
        private bool _hostActive = true;
        private int _hostInactiveCounter = 0;

        private Thread _pingThread;
        private bool _runPingThread;

        private Thread _sendHeartbeatThread;
        private bool _runSendHeartbeatThread;

        private IGameConfigurationHandler _gameConfigurationHandler;
        public string GameName { get; set; }
        public bool AllowedToJoin { get; set; }

        public SessionHandler(IClientController clientController, IScreenHandler screenHandler,
            IMessageService messageService, IGameConfigurationHandler gameConfigurationHandler,
            IDatabaseService<PlayerPOCO> playerService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Session);
            _screenHandler = screenHandler;
            _gameConfigurationHandler = gameConfigurationHandler;
            _messageService = messageService;
            _gameConfigurationHandler = gameConfigurationHandler;
            _playerService = playerService;
            AllowedToJoin = true;
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
                StartSendHeartbeatThread();

                SessionDTO receivedSessionDTO =
                    JsonConvert.DeserializeObject<SessionDTO>(packetDTO.HandlerResponse.ResultMessage);
                if (receivedSessionDTO.Clients.Count < MAXPLAYERS)
                {
                    _session = new Session(receivedSessionDTO.Name);

                    _session.SessionId = sessionId;
                    _clientController.SetSessionId(sessionId);
                    _messageService.AddMessage("Joined game with name: " + _session.Name);

                    SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
                    sessionDTO.Clients = new List<string[]>();
                    sessionDTO.Clients.Add(new[] {_clientController.GetOriginId(), userName});
                    sessionDTO.SessionSeed = receivedSessionDTO.SessionSeed;
                    sessionDTO.SessionStarted = receivedSessionDTO.SessionStarted;
                    SendSessionDTO(sessionDTO);
                    joinSession = true;
                }
                else
                {
                    throw new PlayerLimitException("This session is full you cant join, type return");
                }
            }

            return joinSession;
        }

        private void StartSendHeartbeatThread()
        {
            _runSendHeartbeatThread = true;
            _sendHeartbeatThread = new Thread(() =>
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    while (_runSendHeartbeatThread)
                    {
                        if (stopwatch.ElapsedMilliseconds >= INTERVALTIMEPINGTIMER)
                        {
                            SendHeartbeat();
                            stopwatch.Restart();
                        }
                    }
                })
                {Priority = ThreadPriority.Highest, IsBackground = true};
            _sendHeartbeatThread.Start();
        }

        public bool CreateSession(string sessionName, string userName, bool savedGame, string sessionId, int? seed)
        {
            GameName = sessionName;
            _session = new Session(sessionName);
            if (sessionId is null)
            {
                _session.GenerateSessionId();
                _clientController.SetSessionId(_session.SessionId);
                _clientController.CreateHostController();
            }
            else
            {
                _session.SessionId = sessionId;
                _clientController.SetSessionId(_session.SessionId);
                _clientController.CreateHostController();
            }

            _session.AddClient(_clientController.GetOriginId(), userName);

            if (seed != null)
            {
                _session.SessionSeed = seed.Value;
            }
            else
            {
                _session.SessionSeed = new MapFactory().GenerateSeed();
            }

            _session.InSession = true;
            Thread traingThread = new Thread(
                TrainingScenario.StartTraining
            );
            traingThread.Start();


            _session.SavedGame = savedGame;

            if (_screenHandler.Screen is LobbyScreen screen)
            {
                screen.UpdateLobbyScreen(_session.GetAllClients());
            }

            _heartbeatHandler = new HeartbeatHandler(_messageService);
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
                if (packet.Header.Target.Equals(_clientController.GetOriginId()))
                {
                    if (packet.HandlerResponse.ResultMessage.Equals("Not allowed to join this session"))
                    {
                        return KickFromSession(packet);
                    }
                }

                if (packet.Header.Target == "client" || packet.Header.Target == "host")
                {
                    if (sessionDTO.SessionType == SessionType.RequestToJoinSession)
                    {
                        // _screenHandler.TransitionTo(new LobbyScreen());
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

        private HandlerResponseDTO KickFromSession(PacketDTO packet)
        {
            _clientController.SetSessionId(String.Empty);
            var screen = _screenHandler.Screen as SessionScreen;
            screen.UpdateInputMessage(packet.HandlerResponse.ResultMessage);
            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        // public HandlerResponseDTO HandlePacket(PacketDTO packet)
        // {
        //     SessionDTO sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packet.Payload);
        //
        //     if (packet.Header.SessionID == _session?.SessionId)
        //     {
        //         if (packet.Header.Target == _clientController.GetOriginId())
        //         {
        //             if (sessionDTO.SessionType == SessionType.RequestToJoinSession && packet.HandlerResponse.ResultMessage != null)
        //             {
        //                 if (packet.HandlerResponse.ResultMessage.Equals("Not allowed to join saved or running game") &&
        //                     packet.Header.Target.Equals(_clientController.GetOriginId()))
        //                 {
        //                     AllowedToJoin = false;
        //                     _clientController.SetSessionId(String.Empty);
        //                     var screen = _screenHandler.Screen as SessionScreen;
        //                     screen.UpdateInputMessage("Cannot join this session!");
        //                     return new HandlerResponseDTO(SendAction.Ignore, null);
        //                 }
        //
        //                 if (sessionDTO.SessionStarted && AllowedToJoin)
        //                 {
        //                     
        //                     JoinExistingGame(packet);
        //                 }
        //                 else
        //                 {
        //                     _screenHandler.TransitionTo(new LobbyScreen());
        //                 }
        //                 
        //                 // if (AllowedToJoin)
        //                 // {
        //                 //     _screenHandler.TransitionTo(new LobbyScreen());
        //                 // }
        //             }
        //         }
        //
        //         if (packet.Header.Target == "client" || packet.Header.Target == "host")
        //         {
        //             if (sessionDTO.SessionType == SessionType.SendHeartbeat)
        //             {
        //                 return HandleHeartbeat(packet);
        //             }
        //
        //             if (sessionDTO.SessionType == SessionType.NewBackUpHost)
        //             {
        //                 return HandleNewBackupHost(packet);
        //             }
        //         }
        //
        //         if ((packet.Header.Target == "client" || packet.Header.Target == "host" ||
        //              packet.Header.Target == _clientController.GetOriginId()))
        //         {
        //             if (sessionDTO.SessionType == SessionType.RequestToJoinSession)
        //             {
        //                 return AddPlayerToSession(packet);
        //             }
        //             if (sessionDTO.SessionType == SessionType.EditMonsterDifficulty)
        //             {
        //                 return HandleMonsterDifficulty(packet);
        //             }
        //
        //             if (sessionDTO.SessionType == SessionType.EditItemSpawnRate)
        //             {
        //                 return HandleItemSpawnRate(packet);
        //             }
        //         }
        //
        //         if ((packet.Header.Target == "client" || packet.Header.Target == "host" ||
        //              packet.Header.Target == _clientController.GetOriginId())
        //             && sessionDTO.SessionType == SessionType.SendPing)
        //         {
        //             return HandlePingRequest(packet);
        //         }
        //     }
        //     else
        //     {
        //         if ((packet.Header.Target == "client" || packet.Header.Target == "host")
        //             && sessionDTO.SessionType == SessionType.RequestSessions)
        //         {
        //             return HandleRequestSessions();
        //         }
        //         if (packet.Header.Target == _clientController.GetOriginId()
        //             && sessionDTO.SessionType == SessionType.RequestSessions)
        //         {
        //             return AddRequestedSessions(packet);
        //         }
        //     }
        //     return new HandlerResponseDTO(SendAction.Ignore, null);
        // }

        private void JoinExistingGame(PacketDTO packet)
        {
            PacketDTO newPacket = new PacketBuilder().SetTarget("client").SetSessionID(_clientController.SessionId)
                .SetPayload(packet.HandlerResponse.ResultMessage).SetPacketType(PacketType.GameSession).Build();
            ((IPacketHandler) _clientController).HandlePacket(newPacket);
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
                _messageService.AddMessage("Spawnrate set to " + (ItemSpawnRate) spawnrate);
                _gameConfigurationHandler.SetSpawnRate((ItemSpawnRate) spawnrate, _clientController.SessionId);
                return new HandlerResponseDTO(SendAction.SendToClients, packetDto.Payload);
            }

            if (_clientController.IsBackupHost)
            {
                SessionDTO sessionDTO =
                    JsonConvert.DeserializeObject<SessionDTO>(packetDto.HandlerResponse.ResultMessage);
                int spawnrate = int.Parse(sessionDTO.Name);
                _messageService.AddMessage("Spawnrate set to " + (ItemSpawnRate) spawnrate);
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
                    _hostActive = true;
                    _hostInactiveCounter = 0;
                    SwapToHost();
                    StopPingThread();
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
            sessionDTO.SessionId = _session.SessionId;
            sessionDTO.SessionSeed = _session.SessionSeed;
            sessionDTO.SavedGame = _session.SavedGame;
            sessionDTO.SessionStarted = _session.GameStarted;
            sessionDTO.Clients = _session.GetAllClients();
            var jsonObject = JsonConvert.SerializeObject(sessionDTO);
            return new HandlerResponseDTO(SendAction.ReturnToSender, jsonObject);
        }

        private HandlerResponseDTO AddRequestedSessions(PacketDTO packet)
        {
            if (_availableSessions.ContainsKey(packet.Header.SessionID))
            {
                _availableSessions[packet.Header.SessionID] = packet;
            }
            else
            {
                _availableSessions.TryAdd(packet.Header.SessionID, packet);
            }

            if (_screenHandler.Screen is SessionScreen screen)
            {
                var list = _availableSessions.Select(x =>
                    JsonConvert.DeserializeObject<SessionDTO>(x.Value.HandlerResponse.ResultMessage)).ToList();
                screen.UpdateWithNewSession(list);
            }

            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        public HandlerResponseDTO AddPlayerToSession(PacketDTO packet)
        {
            SessionDTO sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packet.Payload);

            if (packet.Header.Target == "host")
            {
                if (_session.GameStarted || _session.SavedGame)
                {
                    if (!PlayerBelongsToSession(sessionDTO.Clients[0][0]))
                    {
                        return new HandlerResponseDTO(SendAction.ReturnToSender,
                            "Not allowed to join this session");
                    }

                    ActiveGameAddsPlayer(sessionDTO);
                    return new HandlerResponseDTO(SendAction.Ignore, null);
                }
                else
                {
                    if (_screenHandler.Screen is LobbyScreen screen)
                    {
                        var updatedClientList = new List<string[]>();
                        updatedClientList.AddRange(_session.GetAllClients());
                        updatedClientList.AddRange(sessionDTO.Clients);
                        screen.UpdateLobbyScreen(updatedClientList);
                    }

                    _session.AddClient(sessionDTO.Clients[0][0], sessionDTO.Clients[0][1]);
                    sessionDTO.Clients = new List<string[]>();

                    sessionDTO.SessionSeed = _session.SessionSeed;

                    foreach (string[] client in _session.GetAllClients())
                    {
                        sessionDTO.Clients.Add(client);
                    }

                    return new HandlerResponseDTO(SendAction.SendToClients, JsonConvert.SerializeObject(sessionDTO));   
                }
            }
            else
            {
                SessionDTO sessionDTOClients =
                    JsonConvert.DeserializeObject<SessionDTO>(packet.HandlerResponse.ResultMessage);
                _session.EmptyClients();

                if (sessionDTOClients.SessionStarted)
                {
                    _screenHandler.TransitionTo(new GameScreen());
                }
                else
                {
                    _screenHandler.TransitionTo(new LobbyScreen());
                }

                _session.SessionSeed = sessionDTOClients.SessionSeed;

                foreach (string[] client in sessionDTOClients.Clients)
                {
                    _session.AddClient(client[0], client[1]);
                }

                if (sessionDTOClients.Clients.Count > 0 && !_clientController.IsBackupHost)
                {
                    if (sessionDTOClients.Clients[1][0].Equals(_clientController.GetOriginId()))
                    {
                        _clientController.IsBackupHost = true;
                        StartPingThread();
                    }
                }

                if (_screenHandler.Screen is LobbyScreen screen)
                {
                    screen.UpdateLobbyScreen(_session.GetAllClients());
                }

                return new HandlerResponseDTO(SendAction.Ignore, null);
            }
        }

        private bool PlayerBelongsToSession(string clientId)
        {
            var allPlayerId = _playerService.GetAllAsync();
            allPlayerId.Wait();
            var result =
                allPlayerId.Result.Any(x =>
                    x.GameGUID == _session.SessionId && x.PlayerGUID == clientId);

            return result;
        }

        // private HandlerResponseDTO HostAddsPlayer(SessionDTO sessionDTO, PacketDTO packet)
        // {
        //     if (_session.SavedGame || _session.GameStarted)
        //     {
        //         return ActiveGameAddsPlayer(sessionDTO, packet);
        //     }
        //     else
        //     {
        //         _session.AddClient(sessionDTO.Clients[0][0], sessionDTO.Clients[0][1]);
        //         sessionDTO.Clients = new List<string[]>();
        //         sessionDTO.SessionSeed = _session.SessionSeed;
        //
        //         foreach (string[] client in _session.GetAllClients())
        //         {
        //             sessionDTO.Clients.Add(client);
        //         }
        //
        //         if (_screenHandler.Screen is LobbyScreen screen)
        //         {
        //             screen.UpdateLobbyScreen(_session.GetAllClients());
        //         }
        //
        //
        //         return new HandlerResponseDTO(SendAction.SendToClients, JsonConvert.SerializeObject(sessionDTO));
        //     }
        // }

        // private void ClientAddsPlayer(SessionDTO sessionDTO, PacketDTO packet)
        // {
        //     _session.EmptyClients();
        //     _session.SessionSeed = sessionDTO.SessionSeed;
        //
        //     _session.AddClient(sessionDTO.Clients[0][0], sessionDTO.Clients[0][1]);
        //     sessionDTO.Clients = new List<string[]>();
        //     sessionDTO.SessionSeed = _session.SessionSeed;
        //     foreach (string[] client in _session.GetAllClients())
        //     {
        //         sessionDTO.Clients.Add(client);
        //     }
        //
        //     if (_screenHandler.Screen is LobbyScreen screen)
        //     {
        //         screen.UpdateLobbyScreen(sessionDTO.Clients);
        //     }
        //     if (sessionDTO.Clients.Count > 0 && !_clientController.IsBackupHost &&
        //         sessionDTO.Clients.Count <= 2)
        //     {
        //         _clientController.IsBackupHost = true;
        //         StartPingThread();
        //     }
        // }

        private void ActiveGameAddsPlayer(SessionDTO sessionDTO)
        {
            // check if ID matches
            var clientId = sessionDTO.Clients[0];


            var allPlayerId = _playerService.GetAllAsync();
            allPlayerId.Wait();
            var result =
                allPlayerId.Result.FirstOrDefault(x =>
                    x.GameGUID == _session.SessionId && x.PlayerGUID == clientId[0]);

            result.Stamina = 100;
            
            _messageService.AddMessage(sessionDTO.Clients[0][1] + " has joined your session!");
            _session.AddClient(sessionDTO.Clients[0][0], sessionDTO.Clients[0][1]);
            sessionDTO.Clients = new List<string[]>();

            sessionDTO.SessionSeed = _session.SessionSeed;
            StartGameDTO startGameDto = null;

            if (GameStarted())
            {
                startGameDto = HandlePlayerLocation(result);
            }

            var jsonObject = JsonConvert.SerializeObject(startGameDto);

            _clientController.SendPayload(jsonObject, PacketType.GameSession);
        }

        public StartGameDTO HandlePlayerLocation(PlayerPOCO result)
        {
            StartGameDTO startGameDTO = new StartGameDTO();
            startGameDTO.ExistingPlayer = result;
            startGameDTO.Seed = _session.SessionSeed;

            var allPlayerId = _playerService.GetAllAsync();
            allPlayerId.Wait();
            var playerLocations = allPlayerId.Result.Where(x => x.GameGUID == _session.SessionId);

            startGameDTO.SavedPlayers = playerLocations.ToList();
            startGameDTO.GameGuid = _session.SessionId;


            return startGameDTO;
        }

        private void StartPingThread()
        {
            _runPingThread = true;
            _pingThread = new Thread(() =>
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    while (_runPingThread)
                    {
                        if (stopwatch.ElapsedMilliseconds >= INTERVALTIMEPINGTIMER)
                        {
                            HostPingEvent();
                            stopwatch.Restart();
                        }
                    }
                })
                {Priority = ThreadPriority.Highest, IsBackground = true};
            _pingThread.Start();
        }

        private void StopPingThread()
        {
            _runPingThread = false;
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

        public void HostPingEvent()
        {
            SendPing();
            Thread.Sleep(WAITTIMEPINGTIMER);
            CheckIfHostActive();
        }

        public void SwapToHost()
        {
            _clientController.CreateHostController();
            _clientController.IsBackupHost = false;

            StopSendHeartbeatThread();

            _messageService.AddMessage("Look at me, I'm the captain (Host) now!");

            var clients = _session.GetAllClients().Select(client => client.First()).ToArray();
            List<string> heartbeatSenders = new List<string>(clients);
            heartbeatSenders.Remove(_clientController.GetOriginId());

            _heartbeatHandler = new HeartbeatHandler(heartbeatSenders, _messageService);

            SessionDTO sessionDTO = new SessionDTO
            {
                SessionType = SessionType.NewBackUpHost,
                Name = "you'are our co-captain (back up host) now!"
            };
            var jsonObject = JsonConvert.SerializeObject(sessionDTO);
            _clientController.SendPayload(jsonObject, PacketType.Session);
        }

        private void StopSendHeartbeatThread()
        {
            _runSendHeartbeatThread = false;
            _sendHeartbeatThread.Join();
        }

        public bool getHostActive()
        {
            return _hostActive;
        }

        public void setHostActive(bool hostActive)
        {
            _hostActive = hostActive;
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