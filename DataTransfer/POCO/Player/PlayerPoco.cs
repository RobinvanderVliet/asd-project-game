using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DataTransfer.POCO.Player
{
    [ExcludeFromCodeCoverage]
    public class PlayerPoco
    {

        public Guid GameGUID { get; set; }
        [BsonId]
        public string PlayerGUID { get; set; }

        //player info
        public String PlayerName { get; set; }
        public int TypePlayer { get; set; }

        //Statistieken
        public int Health { get; set; }

        //location
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }

}