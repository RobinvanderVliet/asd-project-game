using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Session
{
    [ExcludeFromCodeCoverage]
    public class Session
    {
        private string _name;
        public string Name { get => _name; set => _name = value; }

        private string _sessionId;
        public string SessionId { get => _sessionId; set => _sessionId = value; }

        public bool InSession = false;

        private List<string> _joinedClients = new();

        private int _sessionSeed;
        public int SessionSeed { get => _sessionSeed; set => _sessionSeed = value; }

        public Session(string name)
        {
            _name = name;
        }

        public void GenerateSessionId()
        {
            _sessionId = Guid.NewGuid().ToString();
        }

        public void AddClient(string originId)
        {
            _joinedClients.Add(originId);
            // Remove possible duplicates.
            _joinedClients = _joinedClients.Distinct().ToList();
        }

        public List<string> GetAllClients()
        {
            return _joinedClients;
        }

        public void EmptyClients()
        {
            _joinedClients.Clear();
        }
    }
}
