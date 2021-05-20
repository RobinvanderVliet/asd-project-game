using System.Collections.Generic;

namespace Session.DTO
{
    public class StartGameDto
    {
        public string GameName { get; set; }
        public Dictionary<string, int[]> PlayerLocations  {get; set;}
        
    }
}