using LiteDB;
using System;

namespace DatabaseHandler.POCO
{
    public class ItemPoco
    {
        [BsonId]
        public String ItemName { get; set; }

        public int ItemType { get; set; }
    }
}