using System.Collections.Generic;
using DatabaseHandler.POCO;

namespace Session.DTO
{
    [ExcludeFromCodeCoverage]
    public class StartGameDTO
    {
        public string GameGuid { get; set; }
        public Dictionary<string, int[]> PlayerLocations  {get; set;}
        public List<PlayerPOCO> SavedPlayers { get; set; }
        
    }
}