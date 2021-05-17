using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public interface IHostController
    {
        public void ReceivePacket(PacketDTO packet);
        public void SetSessionId(string sessionId);
    }
}
