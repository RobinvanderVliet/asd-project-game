using ASD_project.Network.DTO;

namespace ASD_project.Network
{
    public interface IPacketHandler
    {
        public HandlerResponseDTO HandlePacket(PacketDTO packet);
    }
}
