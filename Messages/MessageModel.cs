using System.Collections.Generic;
using System.Linq;

namespace Messages
{
    public class MessageModel
    {
        private List<string> _messages;

        public MessageModel()
        {
            _messages = new(); 
        }

        public void addMessage(string message)
        {
            _messages.Add(message);
        }

        public Queue<string> GetLatestMessages(int amount)
        {
            Queue<string> result = new();
            for (int x = amount; x > 0 ; x-- ){
                result.Enqueue(_messages.ElementAt(_messages.Count - x));
            }
            return result;
        }
    }
}
