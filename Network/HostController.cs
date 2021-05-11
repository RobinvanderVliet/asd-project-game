using System;
using System.Collections.Generic;
using Network.DTO;
using Newtonsoft.Json;

namespace Network
{
    public class HostController : IPacketListener
    {
        private NetworkComponent _networkComponent;
        private IPacketHandler _client;
        private string _sessionId;
        private List<PacketDTO> _packetQueue;

        public HostController(NetworkComponent networkComponent, IPacketHandler client, string sessionId)
        {
            _networkComponent = networkComponent;
            _client = client;
            _sessionId = sessionId;
            _networkComponent.HostController = this;
            _packetQueue = new();
        }

        public void ReceivePacket(PacketDTO packet)
        {
            if(packet.Header.SessionID == _sessionId || packet.Header.PacketType == PacketType.Session)
            {
                _packetQueue.Add(packet);
                HandleQueue();
            }
            
/*
            PacketType packetType = packet.Header.PacketType;
            string sessionId = packet.Header.SessionID;*/
            
/*            if (packetType == PacketType.GameAvailability)
            {
                PacketDTO packetDto = new PacketBuilder()
                    .SetSessionID(_sessionId)
                    .SetTarget("client")
                    .SetPacketType(PacketType.GameAvailable)
                    .SetPayload(_session.Name)
                    .Build();

                _networkComponent.SendPacket(packetDto);
                return;
            }*/

/*            if (packetType == PacketType.RequestToJoinGame && IsTheSameSession(sessionId))
            {
                AddPlayerToSession(packet);
                return;
            }*/
        }

        private void HandleQueue()
        {
            foreach(var packet in _packetQueue)
            {
                HandlePacket(packet);
                _packetQueue.Remove(packet);
            }
        }

        private void HandlePacket(PacketDTO packet)
        {
            HandlerResponseDTO succesfullyHandledPacket = _client.HandlePacket(packet);
            if (!succesfullyHandledPacket.ReturnToSender)
            {
                packet.Header.Target = "client";
                _networkComponent.SendPacket(packet);
            }
            else
            {
                packet.Header.Target = packet.Header.OriginID;
                _networkComponent.SendPacket(packet);
            }
        }

        internal void SetSessionId(string sessionId)
        {
            _sessionId = sessionId;
        }

        private void AddPlayerToSession(PacketDTO packet)
        {
/*            _session.AddClient(packet.Header.OriginID);
            Console.WriteLine("A new player with the id: " + packet.Header.OriginID + " joined your session.");

            // Notify all clients that a new client joined.
            PacketDTO packetDTO = new PacketBuilder()
                .SetTarget("client")
                .SetSessionID(_session.SessionId)
                .SetPacketType(PacketType.ClientJoinedGame)
                .SetPayload(JsonConvert.SerializeObject(_session.GetAllClients()))
                .Build();*/

/*            _networkComponent.SendPacket(packetDTO);*/
        }

        private bool IsTheSameSession(string sessionId)
        {
            return sessionId == _sessionId;
        }
    }
}
