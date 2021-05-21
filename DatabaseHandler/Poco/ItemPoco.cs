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
