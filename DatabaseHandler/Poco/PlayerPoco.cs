using System;
using LiteDB;

namespace DatabaseHandler.Poco
{
    public class PlayerPoco
    {
        public MainGamePoco GameGuid { get; set; }
        
        [BsonId]
        public Guid PlayerGuid { get; set; }
    }
}