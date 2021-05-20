namespace Network
{
    public class PacketHeaderDTO
    {
        public string Target { get; set; }
        public string OriginID { get; set; }
        public string SessionID { get; set; }
        public PacketType PacketType { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PacketHeaderDTO dTO &&
                   Target == dTO.Target &&
                   OriginID == dTO.OriginID &&
                   SessionID == dTO.SessionID &&
                   PacketType == dTO.PacketType;
        }
    }
}
