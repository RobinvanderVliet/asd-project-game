using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class PlayerItemPOCO
    {
        public string PlayerGUID { get; set; }

        public string GameGUID { get; set; }
        
        [BsonId]
        public string Id = Guid.NewGuid().ToString();
        public string ItemName { get; set; }

        public int ArmorPoints { get; set; }
        
    }
}