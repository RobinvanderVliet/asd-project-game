namespace ASD_Game.Session
{
    public interface IHeartbeatHandler
    {
        public void ReceiveHeartbeat(string clientId);
    }
}
