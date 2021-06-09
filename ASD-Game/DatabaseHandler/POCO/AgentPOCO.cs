 using System;
using System.Collections.Generic;
using LiteDB;


 namespace DatabaseHandler.POCO
{
    public class AgentPOCO
    {
        public List<KeyValuePair<string, string>> AgentConfiguration { get; set; }
        [BsonId] 
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string PlayerGUID { get; set; }
        public string GameGUID { get; set; }
        public bool Activated { get; set; }
}
}