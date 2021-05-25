using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class PlayerItemPOCO
    {
        public string PlayerGUID { get; set; }

        [BsonId]
        public string ItemName { get; set; }
    }
}
