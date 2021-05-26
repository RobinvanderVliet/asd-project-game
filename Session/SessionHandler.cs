using Network;
using Newtonsoft.Json;
using Session.DTO;
using System;
using System.Collections.Generic;
using Network.DTO;
using WorldGeneration;
using DatabaseHandler;
using DatabaseHandler.Poco;
using DatabaseHandler.Services;
using DatabaseHandler.Repository;


namespace Session
{
    public class SessionHandler : IPacketHandler, ISessionHandler
    {
        private IClientController _clientController;
        private Session _session;
        private Dictionary<string, PacketDTO> _availableSessions = new();

        public SessionHandler(IClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Session);
        }

        public void StartSession(string sessionID)
        {
            var dto = SetupGameHost();
            sendGameSessionDTO(dto);

// bericht naar alle clients dat sessie is gestart
        }


        public Boolean JoinSession(string sessionId)
        {
            if (!_availableSessions.TryGetValue(sessionId, out PacketDTO packetDTO))
            {
                Console.WriteLine("Could not find game!");
            }
            else
            {
                SessionDTO sessionDto =
                    JsonConvert.DeserializeObject<SessionDTO>(packetDTO.HandlerResponse.ResultMessage);
                _session = new Session(sessionDto.Name);
                _session.SessionId = sessionId;
                _clientController.SetSessionId(sessionId);
                Console.WriteLine("Trying to join game with name: " + _session.Name);

                SessionDTO sessionDTO = new SessionDTO(SessionType.RequestToJoinSession);
                sessionDTO.ClientIds = new List<string>();
                sessionDTO.ClientIds.Add(_clientController.GetOriginId());
                sessionDTO.SessionSeed = sessionDto.SessionSeed;
                sendSessionDTO(sessionDTO);
                _session.InSession = true;
            }

            return _session.InSession;
        }

        public Boolean CreateSession(string sessionName)
        {
            _session = new Session(sessionName);
            _session.GenerateSessionId();
            _session.AddClient(_clientController.GetOriginId());
            _session.SessionSeed = MapFactory.GenerateSeed();
            _clientController.CreateHostController();
            _clientController.SetSessionId(_session.SessionId);
            _session.InSession = true;

            Console.Out.WriteLine("Created session with the name: " + _session.Name);

            return _session.InSession;
        }

        public void RequestSessions()
        {
            SessionDTO sessionDTO = new SessionDTO(SessionType.RequestSessions);
            sendSessionDTO(sessionDTO);
        }

        public StartGameDto SetupGameHost()
        {
            var tmp = new DbConnection();
            tmp.SetForeignKeys();

            var tmpServicePlayer = new ServicesDb<PlayerPoco>();
            var tmpServiceGame = new ServicesDb<GamePoco>();

            Guid gameGuid = Guid.NewGuid();
            var tmpObject = new GamePoco {GameGUID = gameGuid};
            tmpServiceGame.CreateAsync(tmpObject);


            List<string> allClients = _session.GetAllClients();
            Dictionary<string, int[]> players = new Dictionary<string, int[]>();

            int playerX = 26;
            int playerY = 11;
            foreach (string element in allClients)
            {
                int[] playerPosition = new int[2];
                playerPosition[0] = playerX;
                playerPosition[1] = playerY;
                players.Add(element, playerPosition);
                var tmpPlayer = new PlayerPoco
                    {PlayerGUID = element, GameGUID = gameGuid, PositionX = playerX, PositionY = playerY};
                tmpServicePlayer.CreateAsync(tmpPlayer);

                playerX+=2;
                playerY+=2;
            }

            StartGameDto startGameDto = new StartGameDto();
            startGameDto.GameName = gameGuid.ToString();
            startGameDto.PlayerLocations = players;

            return startGameDto;
        }


        private void sendGameSessionDTO(StartGameDto startGameDto)
        {
            var payload = JsonConvert.SerializeObject(startGameDto);
            _clientController.SendPayload(payload, PacketType.GameSession);
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
                }
            }
            else if (packet.Header.Target == _clientController.GetOriginId())
            {
                if (sessionDTO.SessionType == SessionType.RequestSessions)
                {
                    return addRequestedSessions(packet);
                }

                return new HandlerResponseDTO(SendAction.Ignore, null);
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
            Console.WriteLine(
                packet.Header.SessionID + " Name: " + sessionDTO.Name + " Seed: " + sessionDTO.SessionSeed);
            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        // private HandlerResponseDTO StartSession(PacketDTO packet)
        // {
        //     if (packet.Header.Target == "host")
        //     {
        //         Console.WriteLine("starting Game");
        //      StartGameDto startGameDto = SetupGameHost();
        //      var jsonObject = JsonConvert.SerializeObject(startGameDto);
        //      sendGameSessionDTO(startGameDto);
        //      
        //      return new HandlerResponseDTO(SendAction.SendToClients, jsonObject);
        //      
        //     }
        //     else
        //     {
        //         return new HandlerResponseDTO(SendAction.Ignore, "You're not the host");
        //     }
        // }

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

            SessionDTO sessionDTOClients =
                JsonConvert.DeserializeObject<SessionDTO>(packet.HandlerResponse.ResultMessage);
            _session.EmptyClients();
            _session.SessionSeed = sessionDTO.SessionSeed;
            Console.Out.WriteLine(_session.SessionSeed);
            Console.Out.WriteLine("Players in your session:");
            foreach (string client in sessionDTOClients.ClientIds)
            {
                _session.AddClient(client);
                Console.Out.WriteLine(client);
            }

            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        public int GetSessionSeed()
        {
            return _session.SessionSeed;
        }
    }
}