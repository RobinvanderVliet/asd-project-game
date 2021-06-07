using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ASD_project.Session.DTO
{
    [ExcludeFromCodeCoverage]
    public class StartGameDTO
    {
        public string GameGuid { get; set; }
        public Dictionary<string, int[]> PlayerLocations { get; set; }

    }
}