using System.Linq;
using ASD_Game.Chat.DTO;
using ASD_Game.Messages;
using ASD_Game.Network;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;
using ASD_Game.Session;
using ASD_Game.UserInterface;
using ASD_Game.World.Services;
using Castle.Core.Internal;
using Newtonsoft.Json;

namespace ASD_Game.Chat
{
    public class ChatHandler : IPacketHandler, IChatHandler
    {
        private readonly IClientController _clientController;
        private readonly IMessageService _messageService;
        private readonly ISessionHandler _sessionHandler;

        public ChatHandler(IClientController clientController, IMessageService messageService, ISessionHandler sessionHandler)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Chat);
            _messageService = messageService;
            _sessionHandler = sessionHandler;
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
                    return new HandlerResponseDTO(SendAction.SendToClients, null);
                case ChatType.Shout:
                    HandleShout(chatDTO.Message, chatDTO.OriginId);
                    return new HandlerResponseDTO(SendAction.SendToClients, null);
            }
            return new HandlerResponseDTO(SendAction.Ignore, null);
        }

        private void HandleSay(string message, string originId)
        {
            _messageService.AddMessage($"{GetUserIdentifier(originId)} said: {message}");
        }

        private void HandleShout(string message, string originId)
        {
            _messageService.AddMessage($"{GetUserIdentifier(originId)} shouted: {message}");
        }

        private string GetUserIdentifier(string userId)
        {
            var player = _sessionHandler.GetAllClients().Find(client => client[0].Equals(userId));
            if (!player[1].IsNullOrEmpty())
            {
                return player[1];
            }

            return $"player with id '{userId}'";
        }
    }
}
