using System;
using System.Collections;
using System.Collections.Generic;

namespace Network
{
    public class ClientComponent : IPacketHandler
    {
        private NetworkComponent _networkComponent;
        private HostComponent _hostComponent;
        private SessionComponent _session;
        private Dictionary<string, PacketDTO> _availableGames = new();

        public ClientComponent(NetworkComponent networkComponent)
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

            return true;
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

        public void JoinGame(string sessionId)
        {
            PacketDTO packetDto;
            
            if (!_availableGames.TryGetValue(sessionId, out packetDto)) {
                Console.WriteLine("Could not find game!");
                return;
            }
            
            _session = new SessionComponent(packetDto.Payload);
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
    }
}
