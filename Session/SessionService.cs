using System;

namespace Session
{
    public class SessionService : ISessionService
    {
        private ISessionHandler _sessionHandler;
        private readonly IGameSessionHandler _gameSessionHandler; 
        public bool inSession { get; set; }

        public bool inGame { get; set; }

        public SessionService(ISessionHandler sessionHandler, IGameSessionHandler gameSessionHandler)
        {
            _sessionHandler = sessionHandler;
            _gameSessionHandler = gameSessionHandler;
        }

        public void getSessionName()
        {
            
        }
        
        public void CreateSession(string messageValue)
        {
            inSession = _sessionHandler.CreateSession(messageValue);
        }

        public void JoinSession(string messageValue)
        {
           inSession = _sessionHandler.JoinSession(messageValue);
        }
        
        public void RequestSessions()
        {
            _sessionHandler.RequestSessions();
        }

        public void StartSession()
        {
            _gameSessionHandler.SendGameSession(_sessionHandler);
        }

    }
}