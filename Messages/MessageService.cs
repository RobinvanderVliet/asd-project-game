using System;

namespace Messages
{
    public class MessageService : IMessageService
    {
        private MessageModel _messages; 
        //add screenhandler 

        public MessageService()
        {
            _messages = new();
        }

        public void AddMessage(string message)
        {
            _messages.AddMessage(message);
            DisplayMessages();
        }

        public void DisplayMessages()
        {
            var latestMessages = _messages.GetLatestMessages(20);

            //should be replace by _screenHandler.displayMessages(latestMessages)
            Console.WriteLine(latestMessages.Peek());
        }
    }
}
