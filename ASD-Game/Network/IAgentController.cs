using Creature.Creature;
using Network.DTO;

namespace Network
{
    public interface IAgentController : IPacketHandler
    {
        public ICreature CreateAgent(string sessionId);
        public void HandlePacket(PacketDTO packetDto);
        // HandlerResponseDTO IPacketHandler.HandlePacket(PacketDTO packet);
        public void setSessionId(string sessionId);
    }
}