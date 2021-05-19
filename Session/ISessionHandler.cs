namespace Session
{
    public interface ISessionHandler
    {
        public void JoinSession(string sessionId);
        public void CreateSession(string sessionName);
        public void RequestSessions();

        //public void SendPing();
    }
}