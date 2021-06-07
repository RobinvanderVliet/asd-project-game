using LiteDB;
using System;

namespace DatabaseHandler.POCO
{
    public class AgentPoco
    {
        [BsonId] public string FileName { get; set; }

        public Guid PlayerGUID { get; set; }
        public Guid GameGUID { get; set; }
    }
}