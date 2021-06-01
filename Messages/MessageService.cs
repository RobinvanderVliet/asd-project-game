using System;

namespace Messages
{
    public class MessageService : IMessageService
    {
        private MessageModel _messages; 
        //add screenhandler 

        public void AddMessage(string message)
        {
            _messages.addMessage(message);
            //refresh screenHandler
        }

        // function that calls _messages.GetLatestMessages(20) and sends it to screenHandler


    }
}
