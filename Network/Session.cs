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
        public string Name { get => _name; }

        private string _sessionId;
        public string SessionId { get => _sessionId; set => _sessionId = value; }

        public Session(string name)
        {
            this._name = name;
            this._sessionId = Guid.NewGuid().ToString();
        }
    }
}
