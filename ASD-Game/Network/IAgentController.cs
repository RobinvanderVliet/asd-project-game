using Network.DTO;

namespace Network
{
    public interface IAgentController : IPacketHandler
    {
        public void HandlePacket(PacketDTO packetDto);
        // HandlerResponseDTO IPacketHandler.HandlePacket(PacketDTO packet);
        public void setSessionId(string sessionId);
    }
}