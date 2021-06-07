using Creature.Creature;
using Network.DTO;
using WorldGeneration;

namespace Network
{
    public interface IAgentController : IPacketHandler
    {
        public Player CreateAgent(string sessionId);
        public void HandlePacket(PacketDTO packetDto);
        // HandlerResponseDTO IPacketHandler.HandlePacket(PacketDTO packet);
        public void setSessionId(string sessionId);
    }
}