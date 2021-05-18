using Network.DTO;

namespace Network
{
    public interface IPacketHandler
    {
        public HandlerResponseDTO HandlePacket(PacketDTO packet);
    }
}
