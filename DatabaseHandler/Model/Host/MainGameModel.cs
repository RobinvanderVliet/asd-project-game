using LiteDB;
using System;

namespace DataBaseHandler.Model.Host
{
    public class MainGameModel
    {
        [BsonId]
        public Guid MainGameGuid { get; set; }
        //public PlayerModel PlayerHostGuid { get; set; }



    }
}