using ASD_Game.Network.DTO;

namespace ASD_Game.Network
{
    public interface INetworkComponent
    {
        public void SendPacket(PacketDTO packet);
        public void SetClientController(IPacketHandler clientController);
        public void SetHostController(IPacketListener hostController);
        public string GetOriginId();
    }
}
