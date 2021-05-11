using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network
{
    public class HostController : IPacketListener
    {
        private NetworkComponent _networkComponent;
        private IPacketHandler _client;
        private Session _session;

        public HostController(NetworkComponent networkComponent, IPacketHandler client, Session session)
        {
            _networkComponent = networkComponent;
            _client = client;
            _session = session;
            _networkComponent.HostController = this;
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

            if (packetType == PacketType.RequestToJoinGame && IsTheSameSession(sessionId))
            {
                _session.AddClient(packet.Header.OriginID);
                Console.WriteLine("A new player with the id: " + packet.Header.OriginID + " joined your session.");
                
                // Notify all clients that a new client joined.
                PacketDTO packetDTO = new PacketBuilder()
                    .SetTarget("client")
                    .SetSessionID(_session.SessionId)
                    .SetPacketType(PacketType.ClientJoinedGame)
                    .SetPayload(JsonConvert.SerializeObject(_session.GetAllClients()))
                    .Build();
                
                _networkComponent.SendPacket(packetDTO);
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

        private bool IsTheSameSession(string sessionId)
        {
            return sessionId == _session.SessionId;
        }
    }
}
