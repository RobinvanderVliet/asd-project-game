namespace ASD_project.Session
{
    public interface IHeartbeatHandler
    {
        public void ReceiveHeartbeat(string clientId);
    }
}
