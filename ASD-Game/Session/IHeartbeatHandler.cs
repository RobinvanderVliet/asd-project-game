
namespace Session
{
    public interface IHeartbeatHandler
    {
        public void ReceiveHeartbeat(string clientId);
    }
}
