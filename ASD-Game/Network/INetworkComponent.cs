using ASD_project.Network.DTO;

namespace ASD_project.Network
{
    public interface INetworkComponent
    {
        public void SendPacket(PacketDTO packet);
        public void SetClientController(IPacketHandler clientController);
        public void SetHostController(IPacketListener hostController);
        public string GetOriginId();
    }
}
