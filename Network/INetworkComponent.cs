using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public interface INetworkComponent
    {
        public void SendPacket(PacketDTO packet);
        public void SetClientController(IPacketHandler clientController);
        public void SetHostController(IPacketListener hostController);
        public string GetOriginId();
    }
}
