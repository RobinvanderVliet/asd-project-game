using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class ClientComponent : IPacketHandler
    {
        private NetworkComponent _networkComponent;
        private HostComponent _hostComponent;
        private SessionComponent _session;


        public ClientComponent()
        {
            this._networkComponent = new NetworkComponent(this);
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
                _networkComponent.SendPayload(packet);
            }
        }

        public void StartGame(NetworkComponent networkComponent, SessionComponent sessionComponent)
        {
            HostComponent hostComponent = new HostComponent(networkComponent, this, sessionComponent);
        }
    }
}
