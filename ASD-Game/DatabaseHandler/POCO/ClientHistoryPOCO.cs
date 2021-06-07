using LiteDB;

namespace DatabaseHandler.POCO
{
    public class ClientHistoryPOCO
    {
        [BsonId]
        public string PlayerId { get; set; }
        public string GameId { get; set; }
    }
}