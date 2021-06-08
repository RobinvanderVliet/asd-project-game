using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace ASD_Game.DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class PlayerPOCO
    {
        public string GameGUID { get; set; }
        public string PlayerGUID { get; set; }
        [BsonId]
        public string GameGUIDAndPlayerGuid { get; set; }
        public string PlayerName { get; set; }
        public int TypePlayer { get; set; }
        public int Health { get; set; }
        public int Stamina { get; set; }
        public int RadiationLevel { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }

    }
}