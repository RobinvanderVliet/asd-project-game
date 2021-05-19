using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace DatabaseHandler.Poco
{
    public class AgentPoco
    {

        [BsonId]
        public String BestandsNaam { get; set; }
        public Guid PlayerGUID { get; set; }
        public Guid GameGUID { get; set; }
    }
}
