using LiteDB;
using System.Diagnostics.CodeAnalysis;

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
