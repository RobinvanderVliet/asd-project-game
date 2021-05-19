using System;
using Session.DTO;

namespace Session
{
    public interface ISessionHandler
    {
        public Boolean JoinSession(string sessionId);
        public Boolean CreateSession(string sessionName);
        public void RequestSessions();
        public void StartSession(string messageValue);
        public StartGameDto SetupGameHost();
    }
}