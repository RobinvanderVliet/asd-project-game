using LiteDB;

namespace DatabaseHandler.Poco
{
    public class PlayerItemPoco
    {
        public string PlayerGUID { get; set; }

        [BsonId]
        public string ItemName { get; set; }
    }
}
