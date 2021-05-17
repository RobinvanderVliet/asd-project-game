using System;
using LiteDB;

namespace DatabaseHandler.Poco
{
    public class MainGamePoco
    {
        [BsonId]
        public Guid MainGameGuid { get; set; }
        //public PlayerModel PlayerHostGuid { get; set; }

    }
}