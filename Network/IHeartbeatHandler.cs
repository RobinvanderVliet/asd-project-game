using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public interface IHeartbeatHandler
    {
        public void RecieveHeartbeat(PacketDTO packet);
    }
}
