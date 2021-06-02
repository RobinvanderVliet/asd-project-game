using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class WorldItemPOCO
    {
        [BsonId]
        public string Id = Guid.NewGuid().ToString();
        public string GameGUID { get; set; }
        public string ItemName { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }

    }
}
