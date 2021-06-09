using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Session.DTO;

namespace ASD_Game.Session.DTO
{
    [ExcludeFromCodeCoverage]
    public class StartGameDTO
    {
        public string GameGuid { get; set; }
        public Dictionary<string, int[]> PlayerLocations { get; set; }
        public AgentConfigurationDTO AgentConfigurationDto { get; set; }
    }
}