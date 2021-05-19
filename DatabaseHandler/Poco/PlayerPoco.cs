using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.Poco
{
    [ExcludeFromCodeCoverage]
    public class PlayerPoco
    {
        public MainGamePoco GameGuid { get; set; }

        public String PlayerName { get; set; }

        [BsonId]
        public Guid PlayerGuid { get; set; }
        
        public int XPosition { get; set; }

        public int YPosition { get; set; }
    }
}