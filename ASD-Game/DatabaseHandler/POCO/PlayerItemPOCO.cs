using LiteDB;
using System.Diagnostics.CodeAnalysis;

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
