using System;
using LiteDB;

namespace DataBaseHandler.Model.Host
{
    public class PlayerModel
    {
        public MainGameModel GameGuid { get; set; }
        public Guid PlayerGuid { get; set; }


        
        public PlayerModel(Guid playerGuid, MainGameModel gameGuid)
        {
            PlayerGuid = playerGuid;
            GameGuid = gameGuid;

        }
        
    }
    
}

