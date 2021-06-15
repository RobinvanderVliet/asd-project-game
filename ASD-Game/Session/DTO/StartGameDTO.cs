using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.DatabaseHandler.POCO;
using Session.DTO;

namespace ASD_Game.Session.DTO
{
    [ExcludeFromCodeCoverage]
    public class StartGameDTO
    {
        public string GameGuid { get; set; }
        public Dictionary<string, int[]> PlayerLocations  {get; set;}
        public List<PlayerPOCO> SavedPlayers { get; set; }
        public List<PlayerItemPOCO> SavedPlayerItems { get; set; }
        public int Seed { get; set; }
        public PlayerPOCO ExistingPlayer { get; set; }

        public Dictionary<string, int[]> PlayerLocations { get; set; }
        public AgentConfigurationDTO AgentConfigurationDto { get; set; }
    }
}