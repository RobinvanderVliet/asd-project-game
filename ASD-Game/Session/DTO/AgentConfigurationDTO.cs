using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Session.DTO
{
    [ExcludeFromCodeCoverage]
    public class AgentConfigurationDTO
    {
        public SessionType SessionType { get; }
        public List<KeyValuePair<string, string>> AgentConfiguration { get; set; }
        public string PlayerId { get; init; }
        public string GameGUID { get; set; }
        public bool Activated { get; set; }

        public AgentConfigurationDTO(SessionType sessionType)
        {
            SessionType = sessionType;
            Activated = false;
        }
    }
}