using ASD_project.Network.DTO;

namespace ASD_project.Network
{
    public interface IPacketListener
    {
        public void ReceivePacket(PacketDTO packet);
    }
}
