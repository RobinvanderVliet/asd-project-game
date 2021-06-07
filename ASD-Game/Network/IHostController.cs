namespace Network
{
    public interface IHostController
    {
        public void ReceivePacket(PacketDTO packet);
        public void SetSessionId(string sessionId);
    }
}
