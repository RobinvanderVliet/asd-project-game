namespace Network
{
    public class PacketHeaderDTO
    {
        public string Target { get; set; }
        public string OriginID { get; set; }
        public string SessionID { get; set; }
        public PacketType PacketType { get; set; }
    }
}
