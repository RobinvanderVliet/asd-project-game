using System.Collections.Generic;

namespace Agent.Models
{
    public class AgentConfiguration : Configuration
    {
        private string _agentName;

        public string AgentName
        {
            get => _agentName;
            set => _agentName = value;
        }
    }
}