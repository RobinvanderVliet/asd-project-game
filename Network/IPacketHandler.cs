using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.DTO;

namespace Network
{
    public interface IPacketHandler
    {
        public HandlerResponseDTO HandlePacket(PacketDTO packet);
    }
}
