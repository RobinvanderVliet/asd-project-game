using System.Diagnostics.CodeAnalysis;
using ASD_Game.Network.DTO;
using ASD_Game.Network.Enum;

namespace ASD_Game.Network
{
    [ExcludeFromCodeCoverage]
    public class PacketBuilder
    {
        readonly PacketHeaderDTO packetHeaderDTO = new PacketHeaderDTO();
        readonly PacketDTO packetDTO = new PacketDTO();

        public PacketBuilder SetTarget(string target)
        {
            packetHeaderDTO.Target = target;
            return this;
        }

        public PacketBuilder SetOriginID(string originId)
        {
            packetHeaderDTO.OriginID = originId;
            return this;
        }

        public PacketBuilder SetSessionID(string sessionID)
        {
            packetHeaderDTO.SessionID = sessionID;
            return this;
        }

        public PacketBuilder SetPacketType(PacketType packetType)
        {
            packetHeaderDTO.PacketType = packetType;
            return this;
        }

        public PacketBuilder SetPayload(string payload)
        {
            packetDTO.Payload = payload;
            return this;
        }

        public PacketDTO Build()
        {
            packetDTO.Header = packetHeaderDTO;
            return packetDTO;
        }
    }
}
