using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace DatabaseHandler.Poco
{
    public class ItemPoco
    {

        [BsonId]
        public string ItemName { get; set; }
        public int ItemType { get; set; }

    }
}
