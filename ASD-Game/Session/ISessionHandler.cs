using System;
using System.Collections.Generic;
using Session.DTO;

using System.Dynamic;

namespace Session
{
    public interface ISessionHandler
    {
        public bool JoinSession(string sessionId, string userName);
        public bool CreateSession(string sessionName, string userName, bool savedGame, string sessionId, int? seed);
        public void RequestSessions();
        public void SendHeartbeat();
        public int GetSessionSeed();
        public List<string[]> GetAllClients();

        public bool GetSavedGame();

        public string GetSavedGameName();

        public bool GameStarted();

        public void SetGameStarted(bool startSessie);

        public string GameName { get; set; }
    }
}