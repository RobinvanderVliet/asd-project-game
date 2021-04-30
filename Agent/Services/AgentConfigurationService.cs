using System.Collections.Generic;
using Agent.Mapper;
using Agent.Models;

namespace Agent.Services
{
    public class AgentConfigurationService : BaseConfigurationService
    {
        private List<AgentConfiguration> _agentConfigurations;

        public AgentConfigurationService()
        {
            Pipeline = new Pipeline();
            FileHandler = new FileHandler();
            FileToDictionaryMapper = new FileToDictionaryMapper();
        }

        public override void CreateConfiguration(string agentName, string filepath)
        {
            var agentConfiguration = new AgentConfiguration();
            agentConfiguration.AgentName = agentName;

            agentConfiguration.Settings = FileToDictionaryMapper.MapFileToConfiguration(filepath);

            _agentConfigurations.Add(agentConfiguration);
        }
    }
}