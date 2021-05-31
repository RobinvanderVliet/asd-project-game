using System;
using System.Diagnostics.CodeAnalysis;
using Network.DTO;

namespace Network
{
    public class HostController : IPacketListener, IHostController
    {
        private INetworkComponent _networkComponent;
        private IPacketHandler _client;
        private string _sessionId;

        public HostController(INetworkComponent networkComponent, IPacketHandler client, string sessionId)
        {
            _networkComponent = networkComponent;
            _client = client;
            _sessionId = sessionId;
            _networkComponent.SetHostController(this);
        }

        public void ReceivePacket(PacketDTO packet)
        {
            // Console.WriteLine("----");
            // Console.WriteLine(packet.Header.SessionID);
            // Console.WriteLine("----");
            // Console.WriteLine(_sessionId);
            // Console.WriteLine("----");
            if(packet.Header.SessionID == _sessionId || packet.Header.PacketType == PacketType.Session)
            {
                HandlePacket(packet);
            }
        }

        private void HandlePacket(PacketDTO packet)
        {
            // Console.WriteLine("handlepacket");
            HandlerResponseDTO handlerResponse = _client.HandlePacket(packet);
            packet.Header.SessionID = _sessionId;

            
            if (handlerResponse.Action == SendAction.SendToClients)
            {
                packet.Header.Target = "client";
                packet.HandlerResponse = handlerResponse;
                _networkComponent.SendPacket(packet);
            }
            else if (handlerResponse.Action == SendAction.ReturnToSender)
            {
                packet.Header.Target = packet.Header.OriginID;
                packet.HandlerResponse = handlerResponse;
                _networkComponent.SendPacket(packet);
            }
        }
        
        [ExcludeFromCodeCoverage]
        public void SetSessionId(string sessionId)
        {
            _sessionId = sessionId;
        }
    }
}
