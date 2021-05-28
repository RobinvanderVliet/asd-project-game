using System.Collections.Generic;
using Session.DTO;

namespace Session
{
    public interface ISessionHandler
    {
        public bool JoinSession(string sessionId);
        public bool CreateSession(string sessionName, bool savedGame, string? sessionId);
        public void RequestSessions();
        public void SendHeartbeat();
        public int GetSessionSeed();
        public List<string> GetAllClients();

        public bool GetSavedGame();

        public string GetSavedGameName();
    }
}