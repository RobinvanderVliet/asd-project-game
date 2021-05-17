using System;

namespace DataBaseHandler.Model.Host
{
    public class MainGameModel
    {
        public Guid GameGuid { get; set; }
        public PlayerModel PlayerHostGuid { get; set; }

    }
}