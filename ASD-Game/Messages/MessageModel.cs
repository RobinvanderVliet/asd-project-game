using System.Collections.Generic;
using System.Linq;

namespace ASD_Game.Messages
{
    public class MessageModel
    {
        private readonly List<string> _messages;

        public MessageModel()
        {
            _messages = new(); 
        }

        public void AddMessage(string message)
        {
            _messages.Add(message);
        }

        public Queue<string> GetLatestMessages(int amount)
        {
            if(_messages.Count < amount)
            {
                amount = _messages.Count;
            }

            Queue<string> result = new();
            for (int x = 0; x < amount ; x++ ){
                result.Enqueue(_messages.ElementAt(_messages.Count - 1 - x));
            }
            return result;
        }
    }
}