using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class PlayerItemPOCO
    {
        [BsonId]
        public string PlayerGUID { get; set; }

        public string ItemName { get; set; }
    }
}
