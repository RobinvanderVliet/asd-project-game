using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace DatabaseHandler.Poco
{
    public class PlayerItemPoco
    {
        public PlayerPoco PlayerGUID { get; set; }

        [BsonId]
        public string ItemName { get; set; }
    }
}
