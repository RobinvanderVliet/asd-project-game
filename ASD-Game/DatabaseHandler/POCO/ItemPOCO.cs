using LiteDB;
using System.Diagnostics.CodeAnalysis;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class ItemPOCO
    {
        [BsonId]
        public string ItemName { get; set; }

        public int ItemType { get; set; }
    }
}