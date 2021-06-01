using System;
using LiteDB;

namespace DatabaseHandler.POCO
{
    public class PlayerItemPoco
    {

        [BsonId]
        public Guid PlayerGUID { get; set; }

        [BsonId]
        public String ItemName { get; set; }
    }
}
