namespace ASD_Game.Chat.DTO
{
    public class ChatDTO
    {
        public ChatType ChatType { get; set; }
        public string Message { get; set; }

        public string OriginId { get; set; }

        public ChatDTO(ChatType chatType, string message)
        {
            ChatType = chatType;
            Message = message;
        }
    }
}
