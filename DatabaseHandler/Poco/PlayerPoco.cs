using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.Poco
{
    [ExcludeFromCodeCoverage]
    public class PlayerPoco
    {

        public GamePoco GameGUID { get; set; }
        [BsonId]
        public string PlayerGUID { get; set; }

        //player info
        public string PlayerName { get; set; }
        public int TypePlayer { get; set; }

        //Statistieken
        public int Health { get; set; }

        //location
        public int XPosition { get; set; }
        public int YPosition { get; set; }
    }

}