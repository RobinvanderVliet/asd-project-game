using System;
using LiteDB;

namespace DataBaseHandler.Model.Host
{
    public class PlayerModel
    {
        public MainGameModel GameGuid { get; set; }
        
        [BsonId]
        public Guid PlayerGuid { get; set; }

        
    }
    
}

