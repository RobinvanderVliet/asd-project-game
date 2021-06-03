using System;
using Network;
using Network.DTO;

namespace WorldGeneration
{
    public class AgentController : IAgentController
    {
        private IClientController _clientController;
        private IWorldService _worldService;

        public AgentController(INetworkComponent networkComponent, IWorldService worldService)
        {
            _worldService = worldService;
            _clientController = new ClientController(networkComponent)
                {AbsoluteOriginId = Guid.NewGuid().ToString(), IsBackupHost = false};
        }

        public void HandlePacket(PacketDTO packet)
        {
        
            //return new(SendAction.Ignore, "result...");
        }

        HandlerResponseDTO IPacketHandler.HandlePacket(PacketDTO packet)
        {
            throw new NotImplementedException();
        }
        
        public void setSessionId(string sessionId)
        {
            _clientController.SetSessionId(sessionId);
        }
    }
}