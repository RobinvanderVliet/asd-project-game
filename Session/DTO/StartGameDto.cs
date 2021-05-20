using System;
using System.Collections.Generic;

namespace Session.DTO
{
    public class StartGameDto
    {
        public string GameGuid { get; set; }
        public Dictionary<string, int[]> PlayerLocations  {get; set;}
        
    }
}