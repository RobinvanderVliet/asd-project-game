using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    class PayloadHeaderDTO
    {
        public string target { get; set; }
        public string originID { get; set; }
        public string sessionID { get; set; }
        public string actionType { get; set; }

        public string payload { get; set; }
    }
}
