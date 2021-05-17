using Network;
using Newtonsoft.Json;
using Session.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                var sesssionDTO = new SessionDTO(SessionType.RequestToJoinSession);
                sendSessionDTO(sesssionDTO);
            }
        }

        public void CreateSession(string sessionName)
        {
            _session = new Session(sessionName);
            _session.GenerateSessionId();
            _session.AddClient(_clientController.GetOriginId());
            _clientController.CreateHostController();
            _clientController.SetSessionId(_session.SessionId);
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
                        return addPlayerToSession(packet);
                    case SessionType.ClientJoinedSession:
                        return clientJoinedSession(packet);
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
            _availableSessions.Add(packet.Header.SessionID, packet);
            var sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packet.HandlerResponse.ResultMessage);
            Console.WriteLine(packet.Header.SessionID + " Name: " + sessionDTO.Name); //TODO add to output
            return new HandlerResponseDTO(false, null);
        }

        private HandlerResponseDTO addPlayerToSession(PacketDTO packet)
        {
            _session.AddClient(packet.Header.OriginID);
            Console.WriteLine("A new player with the id: " + packet.Header.OriginID + " joined your session."); //TODO add to output
            var sessionDTO = new SessionDTO(SessionType.ClientJoinedSession);
            sessionDTO.ClientIds = _session.GetAllClients();
            var jsonObject = JsonConvert.SerializeObject(sessionDTO);
            return new HandlerResponseDTO(false, jsonObject);
        }

        private HandlerResponseDTO clientJoinedSession(PacketDTO packet)
        {
            var sessionDTO = JsonConvert.DeserializeObject<SessionDTO>(packet.HandlerResponse.ResultMessage);
            if (sessionDTO.ClientIds != null)
            {
                foreach (string client in sessionDTO.ClientIds)
                {
                    _session.AddClient(client);
                }
                Console.WriteLine("Clients in current session:");
                int index = 1;
                foreach (string client in _session.GetAllClients())
                {
                    Console.WriteLine(index + ". " + client);
                    index++;
                }
            }
            return new HandlerResponseDTO(false, null);
        }
    }
}
