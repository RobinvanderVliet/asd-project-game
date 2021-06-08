namespace ASD_Game.Chat.DTO
{
    public class ChatMessageDTO
    {
        public string UserName { get; set; }
        public string Message { get; set; }

        public ChatMessageDTO(string userName, string message)
        {
            UserName = userName;
            Message = message;
        }
    }
}
