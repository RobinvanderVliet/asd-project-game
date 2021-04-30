using System;

namespace Network
{
    public class ClientComponent : IPacketHandler
    {
        private NetworkComponent _networkComponent;
        private HostComponent _hostComponent;
        private SessionComponent _session;

        public ClientComponent(NetworkComponent networkComponent)
        {
            _networkComponent = networkComponent;
        }

        public bool HandlePacket(PacketDTO packet)
        {
            // check for session id
            throw new NotImplementedException();
        }

        public void SendPayload(string payload, PacketType packetType)
        {
            PacketDTO packet = new PacketDTO();
            
            packet.Header = new PacketHeaderDTO();
            packet.Header.PacketType = packetType;
            packet.Header.Target = "host"; // Make target into enum
            packet.Payload = payload;


            if (_hostComponent != null)
            {
                _hostComponent.ReceivePacket(packet); //host must check for session?
            }
            else
            {
                packet.Header.SessionID = _session.SessionId; 
                _networkComponent.SendPacket(packet);
            }
        }

        public void CreateGame(string sessionName)
        {
            _session = new SessionComponent(sessionName);
            _session.GenerateSessionId();
            _hostComponent = new HostComponent(_networkComponent, this, _session);
        }
    }
}
