using ASD_Game.Network.DTO;

namespace ASD_Game.Network
{
    public interface IPacketHandler
    {
        public HandlerResponseDTO HandlePacket(PacketDTO packet);
    }
}
