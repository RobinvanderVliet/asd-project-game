using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.Poco
{
    [ExcludeFromCodeCoverage]
    public class PlayerPoco
    {

        public Guid GameGUID { get; set; }
        [BsonId]
        public Guid PlayerGUID { get; set; }

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