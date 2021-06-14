using ASD_Game.Network.DTO;

namespace ASD_Game.Network
{
    public interface IHostController
    {
        public void ReceivePacket(PacketDTO packet);
        public void SetSessionId(string sessionId);
    }
}
