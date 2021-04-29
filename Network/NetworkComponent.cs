using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class NetworkComponent : IPacketListener
    {
        private WebSocketConnection _webSocketConnection;
        private string _originId;
        private IPacketListener _host;
        private IPacketHandler _client;

        public NetworkComponent(IPacketHandler client)
        {
            this._webSocketConnection = new WebSocketConnection(this);
            this._originId = Guid.NewGuid().ToString();
            this._client = client;
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
