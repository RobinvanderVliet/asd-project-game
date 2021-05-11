using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DTO
{
    public class ChatDTO
    {
        public ChatType ChatType { get; set; }
        public string Message { get; set; }

        public ChatDTO(ChatType chatType, string message)
        {
            ChatType = chatType;
            Message = message;
        }
    }
}
