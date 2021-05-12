using System.Diagnostics.CodeAnalysis;

namespace Network
{
    [ExcludeFromCodeCoverage]
    public class PayloadHeaderDTO
    {
        public string Target { get; set; }
        public string OriginID { get; set; }
        public string SessionID { get; set; }
        public string ActionType { get; set; }
    }
}
