using ASD_project.UserInterface;
using Messages;

namespace ASD_project.Messages
{
    public class MessageService : IMessageService
    {
        private MessageModel _messages;
        private IScreenHandler _screenHandler;
        //add screenhandler 

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
