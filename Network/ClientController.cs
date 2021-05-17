using System;
using System.Collections.Generic;
using Network.DTO;
using Newtonsoft.Json;

namespace Network
{
    public class ClientController : IPacketHandler, IClientController
    {
        private INetworkComponent _networkComponent;
        private HostController _hostController;
        private string _sessionId;
        private Dictionary<PacketType, IPacketHandler> _subscribers = new();

        public ClientController(INetworkComponent networkComponent)
        {
            _networkComponent = networkComponent;
            _networkComponent.SetClientController(this);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            if(packet.Header.SessionID == _sessionId || packet.Header.PacketType == PacketType.Session)
            {
                return _subscribers.GetValueOrDefault(packet.Header.PacketType).HandlePacket(packet);
            }

            return new HandlerResponseDTO(SendAction.ReturnToSender, null);
        }

        public void SetSessionId(string sessionId)
        {
            _sessionId = sessionId;
            if(_hostController != null)
            {
                _hostController.SetSessionId(sessionId);
            }
        }

        public void CreateHostController()
        {
            _hostController = new HostController(_networkComponent, this, _sessionId);
        }

        public string GetOriginId()
        {
            return _networkComponent.GetOriginId();
        }

        public void SendPayload(string payload, PacketType packetType)
        {
            if (String.IsNullOrEmpty(payload))
            {
                throw new Exception("Payload is empty.");
            }

            PacketDTO packet = new PacketBuilder()
                .SetTarget("host")
                .SetPacketType(packetType)
                .SetPayload(payload)
                .SetSessionID(_sessionId)
                .Build();

            if (_hostController != null)
            {
                _hostController.ReceivePacket(packet);
            }
            else
            {
                _networkComponent.SendPacket(packet);
            }
        }

        public void SubscribeToPacketType(IPacketHandler packetHandler, PacketType packetType)
        {
            _subscribers.Add(packetType, packetHandler);
        }
    }
}