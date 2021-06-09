using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace ASD_Game.DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class AgentPOCO
    {
        public List<KeyValuePair<string, string>> AgentConfiguration { get; set; }
        [BsonId] public string id { get; set; } = Guid.NewGuid().ToString();
        public string PlayerGUID { get; set; }
        public string GameGUID { get; set; }
        public bool Activated { get; set; }
    }
}