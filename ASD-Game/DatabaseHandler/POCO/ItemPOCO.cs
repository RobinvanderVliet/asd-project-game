using System;
using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace ASD_project.DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class ItemPoco
    {

        [BsonId]
        public String ItemName { get; set; }
        public int Xposition { get; set; }
        public int Yposition { get; set; }
    }
}
