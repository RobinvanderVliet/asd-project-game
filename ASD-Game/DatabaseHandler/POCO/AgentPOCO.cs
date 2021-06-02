using LiteDB;
using System.Diagnostics.CodeAnalysis;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class AgentPOCO
    {

        [BsonId]
        public string FileName { get; set; }
        public PlayerPOCO PlayerGUID { get; set; }
        public GamePOCO GameGUID { get; set; }
    }
}
