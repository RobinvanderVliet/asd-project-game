using System.Diagnostics.CodeAnalysis;
using Network.DTO;

namespace Network
{
    public class HostController : IPacketListener, IHostController
    {
        private INetworkComponent _networkComponent;
        private IPacketHandler _client;
        private IHeartbeatHandler _heartbeat;
        private string _sessionId;

        public HostController(INetworkComponent networkComponent, IPacketHandler client, IHeartbeatHandler heartbeathandler, string sessionId)
        {
            _networkComponent = networkComponent;
            _client = client;
            _sessionId = sessionId;
            _networkComponent.SetHostController(this);
            _heartbeat = heartbeathandler;
        }

        public void ReceivePacket(PacketDTO packet)
        {
            if(packet.Header.SessionID == _sessionId || packet.Header.PacketType == PacketType.Session)
            {
                HandlePacket(packet);
            }
        }

        private void HandlePacket(PacketDTO packet)
        {
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
            else if (handlerResponse.Action == SendAction.Catch)
            {
                _heartbeat.RecieveHeartbeat(packet);
            }
        }

        [ExcludeFromCodeCoverage]
        public void SetSessionId(string sessionId)
        {
            _sessionId = sessionId;
        }
    }
}
