using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.Session.DTO
{
    [ExcludeFromCodeCoverage]
    public class HeartbeatDTO
    {
        public string ClientID { get; set; }
        public bool IsOnline { get; set; }
        public DateTime Time { get; set; }

        public HeartbeatDTO(string clientID)
        {
            this.ClientID = clientID;
            IsOnline = true;
            Time = DateTime.Now;
        }
    }
}
