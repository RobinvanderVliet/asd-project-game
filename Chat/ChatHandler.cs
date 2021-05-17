using Chat.DTO;
using Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.DTO;

namespace Chat
{
    public class ChatHandler : IPacketHandler, IChatHandler
    {
        private IClientController _clientController;

        public ChatHandler(IClientController clientController)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Chat);
        }

        public void SendSay(string message)
        {
            var chatDTO = new ChatDTO(ChatType.Say, message);
            SendChatDTO(chatDTO);
        }

        public void SendShout(string message)
        {
            var chatDTO = new ChatDTO(ChatType.Shout, message);
            SendChatDTO(chatDTO);
        }

        private void SendChatDTO(ChatDTO chatDTO)
        {
            var payload = JsonConvert.SerializeObject(chatDTO);        
            _clientController.SendPayload(payload, PacketType.Chat);
        }

        public HandlerResponseDTO HandlePacket(PacketDTO packet)
        {
            var chatDTO = JsonConvert.DeserializeObject<ChatDTO>(packet.Payload);

            switch (chatDTO.ChatType)
            {
                case ChatType.Say:
                    Console.WriteLine($"say: {chatDTO.Message}");
                    return new HandlerResponseDTO(false, null);
                case ChatType.Shout:
                    Console.WriteLine($"shout: {chatDTO.Message}");
                    return new HandlerResponseDTO(false, null);
            }
            return new HandlerResponseDTO(false, null);
        }
    }
}
