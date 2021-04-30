using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public class ChatHandler : IPacketHandler
    {
        private ClientController _clientController;

        public ChatHandler(ClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Chat);
        }

        public void SendSay(string message)
        {
            _clientController.SendPayload(message, PacketType.Chat);
        }

        public bool HandlePacket(PacketDTO packet)
        {
            Console.WriteLine(packet.Payload);
            return true;
        }

    }
}
