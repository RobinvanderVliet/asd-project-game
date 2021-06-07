using System;
using LiteDB;
using System.Diagnostics.CodeAnalysis;

namespace ASD_project.DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class WorldItemPOCO
    {
        [BsonId]
        public string Id = Guid.NewGuid().ToString();
        public string GameGUID { get; set; }
        public string ItemName { get; set; }
        public int ArmorPoints { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public Guid WorldItemGUID { get; set; }
    }
}
