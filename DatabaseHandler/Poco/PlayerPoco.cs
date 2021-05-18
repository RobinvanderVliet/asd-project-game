using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.Poco
{
    [ExcludeFromCodeCoverage]
    public class PlayerPoco
    {
        public MainGamePoco GameGuid { get; set; }
        
        [BsonId]
        public Guid PlayerGuid { get; set; }
    }
}