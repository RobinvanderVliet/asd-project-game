using Network.DTO;

namespace Network
{
    public class PacketDTO
    {
        public PacketHeaderDTO Header { get; set; }
        public string Payload { get; set; }
        public HandlerResponseDTO HandlerResponse { get; set; }
    }
}
