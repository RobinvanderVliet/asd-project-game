using System;

namespace Session
{
    public class SessionService : ISessionService
    {
        private ISessionHandler _sessionHandler;
        private readonly IGameSessionHandler _gameSessionHandler; 
        public Boolean inSession { get; set; }

        public Boolean inGame { get; set; }

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

        public void StartSession(string messageValue)
        {
            _gameSessionHandler.SendGameSession(messageValue, _sessionHandler);
            //_sessionHandler.StartSession(messageValue);
            inGame = true; 
        }

    }
}