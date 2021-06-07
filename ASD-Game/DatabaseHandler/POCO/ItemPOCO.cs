using LiteDB;
using System;

namespace DatabaseHandler.POCO
{
    public class ItemPOCO
    {
        [BsonId]
        public String ItemName { get; set; }

        public int ItemType { get; set; }
    }
}