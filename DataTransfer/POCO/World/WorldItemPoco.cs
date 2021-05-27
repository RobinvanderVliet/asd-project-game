using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace DataTransfer.POCO.World
{
    public class WorldItemPoco
    {
        [BsonId]
        public Guid GameGUID { get; set; }
        [BsonId]
        public Guid WorldItemGUID { get; set; }
        public String ItemName { get; set; }

    }
}
