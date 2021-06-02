using LiteDB;
using System.Diagnostics.CodeAnalysis;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class PlayerPOCO
    {
        public string GameGuid { get; set; }
        [BsonId]
        public string PlayerGuid { get; set; }
        public string PlayerName { get; set; }
        public int TypePlayer { get; set; }
        public int Health { get; set; }
        public int Stamina { get; set; }
        public int RadiationLevel { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
    }
}