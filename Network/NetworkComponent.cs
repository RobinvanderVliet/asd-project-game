using System;
using Newtonsoft.Json;

namespace Network
{
    public class NetworkComponent : IPacketListener
    {
        private WebSocketConnection _webSocketConnection;
        private string _originId;
        public string OriginId { get => _originId;}
        private IPacketListener _hostController;
        public IPacketListener HostController { get => _hostController; set => _hostController = value; }
        private IPacketHandler _clientController;
        public IPacketHandler ClientController { get => _clientController; set => _clientController = value; }

        public NetworkComponent()
        {
            _webSocketConnection = new WebSocketConnection(this);
            _originId = Guid.NewGuid().ToString();
        }

        public void ReceivePacket(PacketDTO packet)
        {
            if(_hostController != null)
            {
                if(packet.Header.Target == "host")
                {
                    _hostController.ReceivePacket(packet);
                }
            }
            else if(packet.Header.Target == "client" && _clientController != null)
            {
                _clientController.HandlePacket(packet);
            }
        }

        public void SendPacket(PacketDTO packet)
        {
            packet.Header.OriginID = _originId;
            string serializedPacket = JsonConvert.SerializeObject(packet);
            _webSocketConnection.Send(serializedPacket);
        }
    }
}
