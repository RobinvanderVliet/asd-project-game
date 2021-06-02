using Chat.DTO;
using Network;
using Newtonsoft.Json;
using System;
using Network.DTO;
using UserInterface;

namespace Chat
{
    public class ChatHandler : IPacketHandler, IChatHandler
    {
        private IClientController _clientController;
        private IScreenHandler _screenHandler;

        public ChatHandler(IClientController clientController, IScreenHandler screenHandler)
        {
            _clientController = clientController;
            _clientController.SubscribeToPacketType(this, PacketType.Chat);
            _screenHandler = screenHandler;
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
            if (_screenHandler.Screen is GameScreen)
            {
                GameScreen screen = _screenHandler.Screen as GameScreen;
                screen.AddMessage($"{originId} shouted: {message}");
            }
        }

        private void HandleShout(string message, string originId)
        {
            if (_screenHandler.Screen is GameScreen)
            {
                GameScreen screen = _screenHandler.Screen as GameScreen;
                screen.AddMessage($"{originId} shouted: {message}");
            }
        }
    }
}
