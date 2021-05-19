using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.DTO
{
    class HeartbeatDTO
    {
        public string sessionID {get; set;}
        public int status {get; set;}
        public DateTime time {get; set;}

        public HeartbeatDTO(string sessionID)
        {
            this.sessionID = sessionID;
            this.status = 1;
            this.time = DateTime.Now;
        }
    }
}
