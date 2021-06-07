using ASD_project.Chat.DTO;
using ASD_project.Network;
using ASD_project.Network.DTO;
using ASD_project.Network.Enum;
using ASD_project.World.Services;
using Messages;
using Newtonsoft.Json;


namespace ASD_project.Chat
{
    public class ChatHandler : IPacketHandler, IChatHandler
    {
        private readonly IClientController _clientController;
        private readonly IWorldService _worldService;
        private readonly IMessageService _messageService;


        public ChatHandler(IClientController clientController, IWorldService worldService, IMessageService messageService)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Chat);
            _worldService = worldService;
            _messageService = messageService;
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
            var player = _worldService.GetPlayer(userId);
            if (player?.Name != null)
            {
                return player.Name;
            }
            else
            {
                return $"player with id '{userId}'";
            }
        }
    }
}
