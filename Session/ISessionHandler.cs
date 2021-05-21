using System;
using System.Collections.Generic;
using Session.DTO;

namespace Session
{
    public interface ISessionHandler
    {
        public Boolean JoinSession(string sessionId);
        public Boolean CreateSession(string sessionName);
        public void RequestSessions();
        public int GetSessionSeed();
        public List<string> GetAllClients();
    }
}