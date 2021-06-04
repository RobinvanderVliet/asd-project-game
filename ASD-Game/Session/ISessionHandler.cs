using System;
using System.Collections.Generic;

namespace Session
{
    public interface ISessionHandler
    {
        public bool JoinSession(string sessionId, string userName);
        public bool CreateSession(string sessionName, string userName);
        public void RequestSessions();
        public void SendHeartbeat();
        public int GetSessionSeed();
        public List<string[]> GetAllClients();
    }
}