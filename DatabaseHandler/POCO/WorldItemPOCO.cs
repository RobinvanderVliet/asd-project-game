using System;
using LiteDB;

namespace DatabaseHandler.POCO
{
    public class WorldItemPoco
    {
        [BsonId]
        public Guid GameGUID { get; set; }
        [BsonId]
        public Guid WorldItemGUID { get; set; }
        public String ItemName { get; set; }

    }
}
