using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Session.DTO
{
    [ExcludeFromCodeCoverage]
    public class AgentConfigurationDTO
    {
        public SessionType SessionType { get; }
        public List<ValueTuple<string, string>> AgentConfiguration { get; init; }

        public AgentConfigurationDTO(SessionType sessionType)
        {
            SessionType = sessionType;
        }
    }
}