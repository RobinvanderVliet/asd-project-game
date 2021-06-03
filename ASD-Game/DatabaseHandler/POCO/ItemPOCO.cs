using System;
using LiteDB;

namespace DatabaseHandler.POCO
{
    public class ItemPoco
    {

        [BsonId]
        public String ItemName { get; set; }
        public int Xposition { get; set; }
        public int Yposition { get; set; }
    }
}
