using System.Diagnostics.CodeAnalysis;

namespace Network
{
    [ExcludeFromCodeCoverage]
    public class ObjectPayloadDTO
    {
        public PayloadHeaderDTO Header { get; set; }
        public string Payload { get; set; }
    }
}
