using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DataTransfer.POCO.World
{


    [ExcludeFromCodeCoverage]
    public class GamePoco
    {
        [BsonId]
        public Guid GameGUID { get; set; }
        [BsonId]
        public Guid PlayerGUIDHost { get; set; }
        public int Seed { get; set; }

    }
}