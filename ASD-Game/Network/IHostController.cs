namespace Network
{
    public interface IHostController
    {
        public void ReceivePacket(PacketDTO packet);
        public void SetSessionId(string sessionId);
        public void AddAgentController(IAgentController agentController);
    }
}
