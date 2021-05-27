using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace DataTransfer.POCO.Player
{
    public class PlayerItemPoco
    {

        [BsonId]
        public Guid PlayerGUID { get; set; }

        [BsonId]
        public String ItemName { get; set; }
    }
}
