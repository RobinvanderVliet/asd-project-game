using Network.DTO;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace Network
{
    public class HostController : IPacketListener, IHostController
    {
        private INetworkComponent _networkComponent;
        private IPacketHandler _client;
        private string _sessionId;
        private List<IAgentController> _agentControllers;

        public HostController(INetworkComponent networkComponent, IPacketHandler client, string sessionId)
        {
            _networkComponent = networkComponent;
            _client = client;
            _sessionId = sessionId;
            _networkComponent.SetHostController(this);
            _agentControllers = new List<IAgentController>();
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
            SendToAgents(packet);
            
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
        
        private void SendToAgents(PacketDTO packet)
        {
            foreach (var agentController in _agentControllers)
            {
                agentController.HandlePacket(packet);
            }
        }
        
        [ExcludeFromCodeCoverage]
        public void SetSessionId(string sessionId)
        {
            _sessionId = sessionId;
        }

        public void AddAgentController(IAgentController agentController)
        {
            _agentControllers.Add(agentController);
        }
    }
}
