using System.Diagnostics.CodeAnalysis;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;

namespace ASD_Game.Network
{
    public class HostController : IPacketListener, IHostController
    {
        private readonly INetworkComponent _networkComponent;
        private readonly IPacketHandler _client;
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
            if ((packet.Header.SessionID == _sessionId && _sessionId != null) || packet.Header.PacketType == PacketType.Session)
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
        }

        [ExcludeFromCodeCoverage]
        public void SetSessionId(string sessionId)
        {
            _sessionId = sessionId;
        }
    }
}
