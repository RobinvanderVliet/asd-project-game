using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DTO
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
