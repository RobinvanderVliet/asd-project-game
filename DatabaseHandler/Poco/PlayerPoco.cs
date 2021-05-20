using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.Poco
{
    [ExcludeFromCodeCoverage]
    public class PlayerPoco
    {

        public string GameGuid { get; set; }
        [BsonId]
        public string PlayerGuid { get; set; }

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