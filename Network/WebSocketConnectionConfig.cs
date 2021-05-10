using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    [ExcludeFromCodeCoverage]
    public class WebSocketConnectionConfig
    {
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Path { get; set; }
    }
}
