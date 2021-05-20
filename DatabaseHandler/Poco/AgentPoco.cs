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
        public string FileName { get; set; }
        public PlayerPoco PlayerGUID { get; set; }
        public GamePoco GameGUID { get; set; }
    }
}
