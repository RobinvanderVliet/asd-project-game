using System.Collections.Generic;

namespace Session.DTO
{
    public class StartGameDTO
    {
        public string GameGuid { get; set; }
        public Dictionary<string, int[]> PlayerLocations  {get; set;}
        
    }
}