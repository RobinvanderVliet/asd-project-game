using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.Poco
{
    [ExcludeFromCodeCoverage]
    public class GamePoco
    {
        [BsonId]
        public string GameGuid { get; set; } 
       public string PlayerGUIDHost { get; set; }
       public int Seed { get; set; }
    }
}