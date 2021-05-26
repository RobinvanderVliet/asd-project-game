using System;
using System.Diagnostics.CodeAnalysis;

namespace DataTransfer.DTO.Character
{
    [ExcludeFromCodeCoverage]
    public class RelativeStatDTO
    {
        public string GameGuid { get; set; }
        public string PlayerGuid { get; set; }
        public int Health { get; set; }
        public int Stamina { get; set; }
        public int RadiationLevel { get; set; }
    }
}