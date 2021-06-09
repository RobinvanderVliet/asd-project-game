using ASD_Game.Network.DTO;

namespace ASD_Game.Network
{
    public interface IPacketListener
    {
        public void ReceivePacket(PacketDTO packet);
    }
}
