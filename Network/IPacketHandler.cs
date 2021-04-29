using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public interface IPacketHandler
    {
        public bool HandlePacket(PacketDTO packet);
    }
}
