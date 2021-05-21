using System;
using LiteDB;

namespace DatabaseHandler.Poco
{
    public class WorldItemPoco
    {
        public string GameGUID { get; set; }
        [BsonId]
        public string WorldItemGUID { get; set; }
        public string ItemName { get; set; }

    }
}
