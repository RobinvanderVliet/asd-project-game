using System;
using System.Collections.Generic;
using Agent.exceptions;
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
            _agentConfigurations = new List<AgentConfiguration>();
        }

        public override void CreateConfiguration(string agentName, string filepath)
        {
            var agentConfiguration = new AgentConfiguration();
            agentConfiguration.AgentName = agentName;

            agentConfiguration.Settings = FileToDictionaryMapper.MapFileToConfiguration(filepath);

            _agentConfigurations.Add(agentConfiguration);
        }
        
        public List<AgentConfiguration> GetConfigurations()
        {
            return _agentConfigurations;
        }
        
        public void StartConfig()
        {
            StartConfiguration(ConfigurationType.Agent);
        }
    }
}