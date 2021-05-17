using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public interface IChatHandler
    {
        public void SendSay(string message);
        public void SendShout(string message);
    }
}
