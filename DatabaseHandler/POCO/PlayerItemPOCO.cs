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

        public string ItemType { get; set; }

        public string ArmorPartType { get; set; }

        public int ArmorPoints { get; set; }
    }
}

// Armor -> Armorparttype -> body of helmet