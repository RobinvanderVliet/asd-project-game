using Network;
using Newtonsoft.Json;
using Session.DTO;
using System;
using System.Collections.Generic;
using Network.DTO;

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

        public void JoinSession(string sessionId)
        {
            if (!_availableSessions.TryGetValue(sessionId, out PacketDTO packetDTO))
            {
                Console.WriteLine("Could not find game!");
            }
            else
            {
                SessionDTO sessionDto = JsonConvert.DeserializeObject<SessionDTO>(packetDTO.HandlerResponse.ResultMessage);
                _session = new Session(sessionDto.Name);
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
            Console.Out.WriteLine("Created session with the name: " + _session.Name);
        }

        public void RequestSessions()
        {
            var sessionDTO = new SessionDTO(SessionType.RequestSessions);
            sendSessionDTO(sessionDTO);
        }

        private void sendSessionDTO(SessionDTO sessionDTO)
        {
            var payload = JsonConvert.SerializeObject(sessionDTO);
            _clientController.SendPayload(payload, PacketType.Session);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packet.Payload);
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

                        return new HandlerResponseDTO(false, null);
                }
            }
            else
            {
                if (sessionDTO.SessionType == SessionType.RequestSessions)
                {
                    return addRequestedSessions(packet);
                }
            }
            return new HandlerResponseDTO(false, null);
        }

        private HandlerResponseDTO handleRequestSessions()
        {
            var sessionDTO = new SessionDTO(SessionType.RequestSessionsResponse);
            sessionDTO.Name = _session.Name;
            var jsonObject = JsonConvert.SerializeObject(sessionDTO);
            return new HandlerResponseDTO(true, jsonObject);
        }

        private HandlerResponseDTO addRequestedSessions(PacketDTO packet)
        {
            _availableSessions.TryAdd(packet.Header.SessionID, packet);
            var sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packet.HandlerResponse.ResultMessage);
            Console.WriteLine(packet.Header.SessionID + " Name: " + sessionDTO.Name); //TODO add to output
            return new HandlerResponseDTO(false, null);
        }

        private HandlerResponseDTO addPlayerToSession(PacketDTO packet)
        {
            SessionDTO sessionDto = JsonConvert.DeserializeObject<SessionDTO>(packet.Payload);

            if (packet.Header.Target == "host")
            {
                Console.WriteLine(sessionDto.ClientIds[0] + " Has joined your session: ");
                _session.AddClient(sessionDto.ClientIds[0]);
                sessionDto.ClientIds = new List<string>();

                foreach (string client in _session.GetAllClients())
                {
                    sessionDto.ClientIds.Add(client);
                }
                
                
                return new HandlerResponseDTO(false, JsonConvert.SerializeObject(sessionDto));
            }
            
            SessionDTO sessionDtoClients = JsonConvert.DeserializeObject<SessionDTO>(packet.HandlerResponse.ResultMessage);
            _session.EmptyClients();

            Console.Out.WriteLine("Players in your session:");
            foreach (string client in sessionDtoClients.ClientIds)
            {
                _session.AddClient(client);
                Console.Out.WriteLine(client);
            }

            return new HandlerResponseDTO(false, null);
        }
    }
}
