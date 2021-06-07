using System.Collections.Generic;
using DatabaseHandler.POCO;

namespace Session.DTO
{
    public class JoinedPlayerDTO
    {
        public Dictionary<string, int[]> PlayerLocations { get; set; }
        public PlayerPOCO PlayerPoco;
    }
}