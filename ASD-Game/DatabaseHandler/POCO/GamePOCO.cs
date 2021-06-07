using LiteDB;
using System.Diagnostics.CodeAnalysis;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class GamePOCO
    {
        [BsonId]
        public string GameGuid { get; set; }
        public string PlayerGUIDHost { get; set; }
        public int Seed { get; set; }
        
        public string GameName { get; set; }
    }
}