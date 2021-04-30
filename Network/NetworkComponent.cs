using System;
using Newtonsoft.Json;

namespace Network
{
    public class NetworkComponent : IPacketListener
    {
        private WebSocketConnection _webSocketConnection;
        private string _originId;
        public string OriginId { get => _originId;}
        private IPacketListener _host;
        public IPacketListener Host { get => _host; set => _host = value; }
        private IPacketHandler _client;
        public IPacketHandler Client { get => _client; set => _client = value; }

        public NetworkComponent()
        {
            _webSocketConnection = new WebSocketConnection(this);
            _originId = Guid.NewGuid().ToString();
        }

        public void ReceivePacket(PacketDTO packet)
        {
            if(_host != null)
            {
                if(packet.Header.Target == "host")
                {
                    _host.ReceivePacket(packet);
                }
            }
            else if(packet.Header.Target == "client" && _client != null)
            {
                _client.HandlePacket(packet);
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
