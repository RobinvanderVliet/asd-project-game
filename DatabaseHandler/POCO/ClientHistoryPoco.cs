using LiteDB;

namespace DatabaseHandler.POCO
{
    public class ClientHistoryPoco
    {
        [BsonId]
        public string PlayerId { get; set; }
        public string GameId { get; set; }
    }
}