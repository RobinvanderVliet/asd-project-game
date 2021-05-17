using System;
using LiteDB;

namespace DataBaseHandler.Model.Host
{
    public class PlayerModel
    {
        public MainGameModel GameGuid { get; set; }
        public Guid PlayerGuid { get; set; }


        [BsonCtor]
        public PlayerModel(Guid playerGuid, MainGameModel gameGuid)
        {
            playerGuid = playerGuid;
            GameGuid = gameGuid;

        }
        
    }
    
}

