namespace Session
{
    public interface ISessionService
    {
        public void CreateSession(string messageValue);
        
        public void JoinSession(string messageValue);
        
        public void RequestSessions();

        public void StartSession();

        public void StartGame();

        public void LoadGame();

    }
}