using System.Diagnostics.CodeAnalysis;

namespace Network
{
    [ExcludeFromCodeCoverage]
    public class PacketHeaderDTO
    {
        public string Target { get; set; }
        public string OriginID { get; set; }
        public string SessionID { get; set; }
        public PacketType PacketType { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PacketHeaderDTO DTO &&
                   Target == DTO.Target &&
                   OriginID == DTO.OriginID &&
                   SessionID == DTO.SessionID &&
                   PacketType == DTO.PacketType;
        }
        public override int GetHashCode()
        {
            return PacketType.GetHashCode();
        }
    }
}
