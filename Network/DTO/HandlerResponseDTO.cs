using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.DTO
{
    public class HandlerResponseDTO
    {
        public SendAction Action { get; set; }
        public string ResultMessage { get; set; }
    
        public HandlerResponseDTO(SendAction action, string resultMessage)
        {
            Action = action;
            ResultMessage = resultMessage;
        }
    }
}
