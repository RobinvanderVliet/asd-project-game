using System;

namespace Session
{
    public class SessionService : ISessionService
    {
        private ISessionHandler _sessionHandler;
        public Boolean inSession = false; 

        public SessionService(ISessionHandler sessionHandler)
        {
            _sessionHandler = sessionHandler;

        }
        
        public void CreateSession(string messageValue)
        {
            _sessionHandler.CreateSession(messageValue);
        }

        public void JoinSession(string messageValue)
        {
            _sessionHandler.JoinSession(messageValue);
        }
        
        public void RequestSessions()
        {
            _sessionHandler.RequestSessions();
        }

        public void StartSession()
        {
            
        }

        public Boolean InSession()
        {
            return _sessionHandler.InSession();
        }

    }
}