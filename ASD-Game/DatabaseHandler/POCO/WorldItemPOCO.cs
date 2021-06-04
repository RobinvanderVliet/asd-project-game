using System;
using LiteDB;

namespace ASD_project.DatabaseHandler.POCO
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
