using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.Network
{
    [ExcludeFromCodeCoverage]
    public class WebSocketConnectionConfig
    {
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Path { get; set; }
    }
}
