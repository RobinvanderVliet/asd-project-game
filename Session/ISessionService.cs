using System;

namespace Session
{
    public interface ISessionService
    {
        public Boolean inSession { get; set; }

        public Boolean inGame { get; set; }
        public void CreateSession(string messageValue);
        
        public void JoinSession(string messageValue);
        
        public void RequestSessions();

        public void StartSession(string messageValue);

    }
}