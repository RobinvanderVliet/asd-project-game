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
            HandlerResponseDTO handlerResponse = _client.HandlePacket(packet);
            if (!handlerResponse.ReturnToSender)
            {
                packet.Header.Target = "client";
                _networkComponent.SendPacket(packet);
            }
            else
            {
                packet.Header.Target = packet.Header.OriginID;
                packet.HandlerResponse = handlerResponse;
                _networkComponent.SendPacket(packet);
            }
        }

        public void SetSessionId(string sessionId)
        {
            _sessionId = sessionId;
        }
    }
}
