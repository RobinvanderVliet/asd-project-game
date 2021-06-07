using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.Network.DTO
{
    [ExcludeFromCodeCoverage]
    public class PacketDTO
    {
        public PacketHeaderDTO Header { get; set; }
        public string Payload { get; set; }
        public HandlerResponseDTO HandlerResponse { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PacketDTO DTO &&
                   EqualityComparer<PacketHeaderDTO>.Default.Equals(Header, DTO.Header) &&
                   Payload == DTO.Payload &&
                   EqualityComparer<HandlerResponseDTO>.Default.Equals(HandlerResponse, DTO.HandlerResponse);
        }
        public override int GetHashCode()
        {
            return Payload.GetHashCode();
        }
    }
}
