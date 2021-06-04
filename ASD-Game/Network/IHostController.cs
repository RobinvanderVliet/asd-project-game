using ASD_project.Network.DTO;

namespace ASD_project.Network
{
    public interface IHostController
    {
        public void ReceivePacket(PacketDTO packet);
        public void SetSessionId(string sessionId);
    }
}
