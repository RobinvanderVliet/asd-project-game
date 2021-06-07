using LiteDB;
using System.Diagnostics.CodeAnalysis;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class PlayerPOCO
    {
        public string GameGuid { get; set; }
        public string PlayerGuid { get; set; }

        [BsonId]
        public string GameGUIDAndPlayerGuid { get; set; }

        public string PlayerName { get; set; }
        public int TypePlayer { get; set; }
        public int Health { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int Stamina { get; set; }
    }
}