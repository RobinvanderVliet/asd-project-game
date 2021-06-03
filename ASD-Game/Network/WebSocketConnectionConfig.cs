using System.Diagnostics.CodeAnalysis;

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
