using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace DatabaseHandler.Poco
{
    public class WorldItemPoco
    {
        public GamePoco GameGUID { get; set; }
        [BsonId]
        public Guid WorldItemGUID { get; set; }
        public String ItemName { get; set; }

    }
}
