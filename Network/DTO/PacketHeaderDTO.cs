namespace Network
{
    public class HeartbeatDTO
    {
        public string Target { get; set; }
        public string OriginID { get; set; }
        public string SessionID { get; set; }
        public PacketType PacketType { get; set; }

        public override bool Equals(object obj)
        {
            return obj is HeartbeatDTO dTO &&
                   Target == dTO.Target &&
                   OriginID == dTO.OriginID &&
                   SessionID == dTO.SessionID &&
                   PacketType == dTO.PacketType;
        }
    }
}
