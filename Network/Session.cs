using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class Session
    {
        private string _name;
        public string Name { get => _name; set => _name = value; }

        private string _sessionId;
        public string SessionId { get => _sessionId; set => _sessionId = value; }
        private List<string> _joinedClients = new();

        public Session()
        {
            
        }

        public Session(string name)
        {
            this._name = name;
            this._sessionId = Guid.NewGuid().ToString();
        }

        public void GenerateSessionId()
        {
            _sessionId = Guid.NewGuid().ToString();
        }

        public void AddClient(string originId)
        {
            _joinedClients.Add(originId);
        }

        public List<string> GetAllClients()
        {
            return _joinedClients;
        }
    }
}
