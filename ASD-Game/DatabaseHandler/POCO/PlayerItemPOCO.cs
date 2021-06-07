using LiteDB;
using System;

namespace DatabaseHandler.POCO
{
    public class PlayerItemPoco
    {
        public string PlayerGUID { get; set; }
        public string GameGUID { get; set; }

        [BsonId]
        public string Id = Guid.NewGuid().ToString();

        public string ItemName { get; set; }
        public int ArmorPoints { get; set; }
    }
}