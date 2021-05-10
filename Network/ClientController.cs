using System;
using System.Collections;
using System.Collections.Generic;

namespace Network
{
    public class ClientController : IPacketHandler
    {
        private NetworkComponent _networkComponent;
        private HostController _hostController;
        private Session _session;
        private Dictionary<string, PacketDTO> _availableGames = new();
        private Dictionary<PacketType, IPacketHandler> _subscribers = new();

        public ClientController(NetworkComponent networkComponent)
        {
            _networkComponent = networkComponent;
        }

        public bool HandlePacket(PacketDTO packet)
        {
            if (packet.Header.PacketType == PacketType.GameAvailable)
            {
                _availableGames.Add(packet.Header.SessionID, packet);
                Console.WriteLine(packet.Header.SessionID);
                return true;
            }
            else
            {
                return _subscribers.GetValueOrDefault(packet.Header.PacketType).HandlePacket(packet);
            }
        }

        public void SendPayload(string payload, PacketType packetType)
        {
            PacketDTO packet = new PacketBuilder()
                .SetTarget("host")
                .SetPacketType(packetType)
                .SetPayload(payload)
                .Build();

            if (_hostController != null)
            {
                _hostController.ReceivePacket(packet); //host must check for session?
            }
            else
            {
                packet.Header.SessionID = _session.SessionId; 
                _networkComponent.SendPacket(packet);
            }
        }

        public void CreateGame(string sessionName)
        {
            _session = new Session(sessionName);
            _session.GenerateSessionId();
            _hostController = new HostController(_networkComponent, this, _session);
        }

        public void JoinGame(string sessionId)
        {
            PacketDTO packetDto;
            
            if (!_availableGames.TryGetValue(sessionId, out packetDto)) {
                Console.WriteLine("Could not find game!");
                return;
            }
            
            _session = new Session(packetDto.Payload);
            _session.SessionId = sessionId;
            Console.WriteLine("You joined game: " + _session.Name);
        }

        public void FindGames()
        {
            PacketDTO packetDTO = new PacketBuilder()
                .SetTarget("host")
                .SetPacketType(PacketType.GameAvailability)
                .SetPayload("testPayload")
                .Build();

            _networkComponent.SendPacket(packetDTO);
        }

        public void SubscribeToPacketType(IPacketHandler packetHandler, PacketType packetType)
        {
            _subscribers.Add(packetType, packetHandler);
        }
    }
}
