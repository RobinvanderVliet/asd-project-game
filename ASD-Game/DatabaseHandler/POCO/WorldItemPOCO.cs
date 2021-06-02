using LiteDB;
using System.Diagnostics.CodeAnalysis;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class WorldItemPOCO
    {
        public string GameGUID { get; set; }
        [BsonId]
        public string WorldItemGUID { get; set; }
        public string ItemName { get; set; }

    }
}
