using System;
using LiteDB;

namespace ASD_project.DatabaseHandler.POCO
{
    public class PlayerItemPoco
    {

        [BsonId]
        public Guid PlayerGUID { get; set; }

        [BsonId]
        public String ItemName { get; set; }
    }
}
