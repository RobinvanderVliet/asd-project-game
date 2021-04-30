using System;
using System.Collections.Generic;

namespace Network
{
    public class HostController : IPacketListener
    {
        private NetworkComponent _networkComponent;
        private IPacketHandler _client;
        private Session _session;
        private List<string> _joinedPlayers;

        public HostController(NetworkComponent networkComponent, IPacketHandler client, Session session)
        {
            _networkComponent = networkComponent;
            _client = client;
            _session = session;
            _networkComponent.Host = this;
        }

        public void ReceivePacket(PacketDTO packet)
        {
            PacketType packetType = packet.Header.PacketType;
            string sessionId = packet.Header.SessionID;
            
            if (packetType == PacketType.GameAvailability)
            {
                PacketDTO packetDto = new PacketBuilder()
                    .SetSessionID(_session.SessionId)
                    .SetTarget("client")
                    .SetPacketType(PacketType.GameAvailable)
                    .SetPayload(_session.Name)
                    .Build();

                _networkComponent.SendPacket(packetDto);
                return;
            }

            if (packetType == PacketType.RequestToJoinGame && isTheSameSession(sessionId))
            {
                _joinedPlayers.Add(packet.Header.OriginID);
                Console.WriteLine("A new player with the id: " + packet.Header.OriginID + " joined your session.");
                
                // TODO: Notify all players that someone new joined the session.
                return;
            }
            
            if(packet.Header.SessionID == _session.SessionId)
            {
                bool success = _client.HandlePacket(packet);
                if (success)
                {
                    packet.Header.Target = "client";
                    _networkComponent.SendPacket(packet);
                }
                else
                {
                    //TODO: send error
                }
            }
        }

        private bool isTheSameSession(string sessionId)
        {
            return sessionId == _session.SessionId;
        }
    }
}
