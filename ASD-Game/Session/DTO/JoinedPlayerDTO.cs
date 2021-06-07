using System.Collections.Generic;
using DatabaseHandler.POCO;
using WorldGeneration;

namespace Session.DTO
{
    public class JoinedPlayerDTO
    {
        public Dictionary<string, int[]> PlayerLocations { get; set; }
        public PlayerPOCO PlayerPoco;
    }
}