using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    class ObjectPayloadDTO
    {
        public PayloadHeaderDTO header { get; set; }
        public string payload { get; set; }
    }
}
