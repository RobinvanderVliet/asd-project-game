using System;
using ASD_Game.Network.DTO;
using Newtonsoft.Json;
using WebSocketSharp;

namespace ASD_Game.Network
{
    public class NetworkComponent : IPacketListener, INetworkComponent
    {
        private IWebSocketConnection _webSocketConnection;
        private string _originId;
        private IPacketListener _hostController;
        private IPacketHandler _clientController;

        public NetworkComponent()
        {
            _webSocketConnection = new WebSocketConnection(this);
            InitializeOriginId();
        }

        public NetworkComponent(IWebSocketConnection webSocketConnection)
        {
            //Constructor solely used for testing purposes
            _webSocketConnection = webSocketConnection;
            _originId = Guid.NewGuid().ToString();
        }

        private void InitializeOriginId()
        {
            if (_webSocketConnection.UserSettingsConfig.OriginId.IsNullOrEmpty())
            {
                _originId = Guid.NewGuid().ToString();
                _webSocketConnection.AddOrUpdateConfigVariables("UserSettingsConfig:OriginId", _originId);
            }
            else
            {
                _originId = _webSocketConnection.UserSettingsConfig.OriginId;
            }
        }

        public void ReceivePacket(PacketDTO packet)
        {
            if (_hostController != null)
            {
                if (packet.Header.Target == "host")
                {
                    _hostController.ReceivePacket(packet);
                }
            }
            else if ((packet.Header.Target == "client" || packet.Header.Target == _originId) && _clientController != null)
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

        public void SetWebSocketConnection(IWebSocketConnection webSocketConnection)
        {
            _webSocketConnection = webSocketConnection;
        }

        public void SetClientController(IPacketHandler clientController)
        {
            _clientController = clientController;
        }

        public void SetHostController(IPacketListener hostController)
        {
            _hostController = hostController;
        }
        
        public string GetOriginId()
        {
            return _originId;
        }
    }
}