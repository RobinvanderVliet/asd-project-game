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
            chatDTO.OriginId = _clientController.GetOriginId();
            SendChatDTO(chatDTO);
        }

        public void SendShout(string message)
        {
            var chatDTO = new ChatDTO(ChatType.Shout, message);
            chatDTO.OriginId = _clientController.GetOriginId();
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
                    HandleSay(chatDTO.Message, chatDTO.OriginId);
                    return new HandlerResponseDTO(false, null);
                case ChatType.Shout:
                    HandleShout(chatDTO.Message, chatDTO.OriginId);
                    return new HandlerResponseDTO(false, null);
            }
            return new HandlerResponseDTO(false, null);
        }

        private void HandleSay(string message, string originId)
        {
            Console.WriteLine($"{originId} said: {message}");
        }

        private void HandleShout(string message, string originId)
        {
            Console.WriteLine($"{originId} shouted: {message}");
        }
    }
}
