using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace DataTransfer.POCO.World
{
    public class ItemPoco
    {

        [BsonId]
        public String ItemName { get; set; }
        public int ItemType { get; set; }

    }
}
