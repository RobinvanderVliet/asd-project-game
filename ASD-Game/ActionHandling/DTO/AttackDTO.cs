using System.Diagnostics.CodeAnalysis;

namespace ActionHandling.DTO
{
    [ExcludeFromCodeCoverage]
    public class AttackDTO
    {
        public string Direction { get; set; }
        public string PlayerGuid { get; set; }
    }
}