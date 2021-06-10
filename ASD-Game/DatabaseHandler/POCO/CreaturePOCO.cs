using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class CreaturePOCO
    {
        public string GameGuid { get; set; }
        [BsonId]
        public string CreatureGuid { get; set; }
        public string TypeCreature { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public int VisionRange { get; set; }
        public bool Following { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
    }
}
