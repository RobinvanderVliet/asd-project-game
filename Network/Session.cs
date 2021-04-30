using System;
using System.Collections.Generic;
using System.Linq;

namespace Network
{
    public class Session
    {
        private string _name;
        public string Name { get => _name; set => _name = value; }

        private string _sessionId;
        public string SessionId { get => _sessionId; set => _sessionId = value; }
        private List<string> _joinedClients = new();

        public Session(string name)
        {
            _name = name;
            _sessionId = Guid.NewGuid().ToString();
        }

        public void GenerateSessionId()
        {
            _sessionId = Guid.NewGuid().ToString();
        }

        public void AddClient(string originId)
        {
            _joinedClients.Add(originId);
            _joinedClients = _joinedClients.Distinct().ToList(); // Remove possible duplicates.
        }

        public List<string> GetAllClients()
        {
            return _joinedClients;
        }
    }
}
