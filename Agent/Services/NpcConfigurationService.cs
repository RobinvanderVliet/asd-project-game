using System.Collections.Generic;
using Agent.Mapper;
using Agent.Models;
using Agent.Services.interfaces;
using Configuration = Agent.Models.Configuration;

namespace Agent.Services
{
    public class NpcConfigurationService : BaseConfigurationService
    {
        private List<Configuration> _npcConfigurations;

        public NpcConfigurationService(List<Configuration> npcConfigurations, FileToDictionaryMapper fileToDictionaryMapper)
        {
            _npcConfigurations = npcConfigurations;
            _fileToDictionaryMapper = fileToDictionaryMapper;
        }

        public override void CreateConfiguration(string npcName, string filepath)
        {
            var npcConfiguration = new NpcConfiguration();
            npcConfiguration.NpcName = npcName;

            npcConfiguration.Settings = _fileToDictionaryMapper.MapFileToConfiguration(filepath);
            
            _npcConfigurations.Add(npcConfiguration);
            
        }

        public override List<Configuration> GetConfigurations()
        {
            return _npcConfigurations;
        }

        public override void StartConfiguration(ConfigurationType configurationType)
        {
            
        }
    }
}