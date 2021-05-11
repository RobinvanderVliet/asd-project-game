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
        private ClientController _clientController;
        private Session _session;
        private Dictionary<string, PacketDTO> _availableSessions = new();

        public SessionHandler(ClientController clientController)
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
                _session = new Session(packetDTO.Payload);
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

            switch (sessionDTO.SessionType)
            {
                case SessionType.RequestSessions:
                    return requestSessions();
                case SessionType.RequestSessionsResponse:
                    return true;
                case SessionType.RequestToJoinSession:
                    return true;
                case SessionType.ClientJoinedSession:
                    return true;
            }
            return true;
        }

        private HandlerResponseDTO requestSessions()
        {
            var sessionDTO = new SessionDTO(SessionType.RequestSessionsResponse, _session.Name);
            var jsonObject = JsonConvert.SerializeObject(sessionDTO);
            return new HandlerResponseDTO(true, jsonObject);
        }
    }
}
