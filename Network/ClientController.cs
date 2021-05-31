using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Network.DTO;

namespace Network
{
    public class ClientController : IPacketHandler, IClientController
    {
        private INetworkComponent _networkComponent;
        private IHostController _hostController;
        private string _sessionId;
        private Dictionary<PacketType, IPacketHandler> _subscribers = new();
        private bool _isBackupHost;
        public bool IsBackupHost { get => _isBackupHost; set => _isBackupHost = value; }
        public string SessionId { get => _sessionId; }

        public ClientController(INetworkComponent networkComponent)
        {
            _networkComponent = networkComponent;
            _networkComponent.SetClientController(this);
            _isBackupHost = false;
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            if(packet.Header.SessionID == _sessionId || packet.Header.PacketType == PacketType.Session)
            {
                return _subscribers.GetValueOrDefault(packet.Header.PacketType).HandlePacket(packet);
            }
            else
            {
                return new HandlerResponseDTO(SendAction.ReturnToSender, null);
            }
        }

        public void SetSessionId(string sessionId)
        {
            _sessionId = sessionId;
            if(_hostController != null)
            {
                _hostController.SetSessionId(sessionId);
            }
        }

        [ExcludeFromCodeCoverage]
        public void CreateHostController()
        {
            _hostController = new HostController(_networkComponent, this, _sessionId);
        }

        [ExcludeFromCodeCoverage]
        public string GetOriginId()
        {
            return _networkComponent.GetOriginId();
        }

        public void SendPayload(string payload, PacketType packetType)
        {
            if (string.IsNullOrEmpty(payload))
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
                Console.WriteLine("test ralifakfj");
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

        public void SetHostController(IHostController hostController)
        {
            _hostController = hostController;
        }

        public bool IsHost()
        {
            return _hostController != null;
        }
    }
}