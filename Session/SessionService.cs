using System;

namespace Session
{
    public class SessionService : ISessionService
    {
        private ISessionHandler _sessionHandler;
        public Boolean inSession { get; set; }

        public SessionService(ISessionHandler sessionHandler)
        {
            _sessionHandler = sessionHandler;

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
            _sessionHandler.StartSession(messageValue);
        }

    }
}