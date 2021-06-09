using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace ASD_Game.DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class AgentPOCO
    {
        [BsonId] public string FileName { get; set; }

        public Guid PlayerGUID { get; set; }
        public Guid GameGUID { get; set; }
    }
}