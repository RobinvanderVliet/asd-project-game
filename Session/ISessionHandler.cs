using System;

namespace Session
{
    public interface ISessionHandler
    {
        public Boolean JoinSession(string sessionId);
        public Boolean CreateSession(string sessionName);
        public void RequestSessions();

    }
}