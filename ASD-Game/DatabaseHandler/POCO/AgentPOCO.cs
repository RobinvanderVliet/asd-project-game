using System;
using System.Collections.Generic;
using LiteDB;
using System.Diagnostics.CodeAnalysis;

namespace DatabaseHandler.POCO
{
    [ExcludeFromCodeCoverage]
    public class AgentPOCO
    {
        public List<ValueTuple<string, string>> AgentConfiguration { get; set; }
        [BsonId] 
        public string id = Guid.NewGuid().ToString();
        public string PlayerGUID { get; set; }
    }
}
