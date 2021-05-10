using Chat.DTO;
using Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public class ChatHandler : IPacketHandler, IChatHandler
    {
        private ClientController _clientController;

        public ChatHandler(ClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Chat);
        }

        public void SendSay(string message)
        {
            var chatDTO = new ChatDTO(ChatType.Say, message);
            SendChat(chatDTO);
        }

        public void SendShout(string message)
        {
            var chatDTO = new ChatDTO(ChatType.Shout, message);
            SendChat(chatDTO);
        }

        private void SendChat(ChatDTO chatDTO)
        {
            var payload = JsonConvert.SerializeObject(chatDTO);
            _clientController.SendPayload(payload, PacketType.Chat);
        }

        public bool HandlePacket(PacketDTO packet)
        {
            var chatDTO = JsonConvert.DeserializeObject<ChatDTO>(packet.Payload);

            switch (chatDTO.ChatType)
            {
                case ChatType.Say:
                    Console.WriteLine($"say: {chatDTO.Message}");
                    return true;
                case ChatType.Shout:
                    Console.WriteLine($"shout: {chatDTO.Message}");
                    return true;
            }
            return true;
        }
    }
}
