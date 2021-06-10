using ASD_Game.UserInterface;

namespace ASD_Game.Messages
{
    public class MessageService : IMessageService
    {
        private readonly MessageModel _messages;
        private readonly IScreenHandler _screenHandler;

        public MessageService(IScreenHandler screenHandler)
        {
            _messages = new();
            _screenHandler = screenHandler;
        }

        public void AddMessage(string message)
        {
            _messages.AddMessage(message);
            DisplayMessages();
        }

        public void DisplayMessages()
        {
            var latestMessages = _messages.GetLatestMessages(40);
            _screenHandler.ShowMessages(latestMessages);
        }
    }
}
