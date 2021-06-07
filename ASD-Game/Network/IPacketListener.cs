
namespace Network
{
    public interface IPacketListener
    {
        public void ReceivePacket(PacketDTO packet);
    }
}
