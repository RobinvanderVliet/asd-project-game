using System.Diagnostics.CodeAnalysis;

namespace ActionHandling.DTO
{
    [ExcludeFromCodeCoverage]
    public class AttackDTO
    {
        public int XPosition { get; set; }
        public int YPosition{ get; set; }
        public int Damage { get; set; }

        public int Stamina { get; set; }

        public string PlayerGuid { get; set; }
        public string AttackedPlayerGuid { get; set; }
    }
}