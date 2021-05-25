using System;
using System.Diagnostics.CodeAnalysis;

namespace Session.DTO
{
    [ExcludeFromCodeCoverage]
    public class HeartbeatDTO
    {
        public string sessionID { get; set; }
        public bool online { get; set; }
        public DateTime time { get; set; }

        public HeartbeatDTO(string sessionID)
        {
            this.sessionID = sessionID;
            online = true;
            time = DateTime.Now;
        }
    }
}
