using System.Collections.Generic;
using Agent.Mapper;
using Agent.Models;
using Agent.Services.interfaces;

namespace Agent.Services
{
    public class NpcConfigurationService : BaseConfigurationService, INpcConfigurationService
    {
        private List<NpcConfiguration> _npcConfigurations;

        public NpcConfigurationService(List<NpcConfiguration> npcConfigurations, FileToDictionaryMapper fileToDictionaryMapper)
        {
            _npcConfigurations = npcConfigurations;
            FileToDictionaryMapper = fileToDictionaryMapper;
        }

        public override void CreateConfiguration(string npcname, string filepath)
        {
            var npcConfiguration = new NpcConfiguration();
            npcConfiguration.NpcName = npcname;

            npcConfiguration.Settings = FileToDictionaryMapper.MapFileToConfiguration(filepath);
            
            _npcConfigurations.Add(npcConfiguration);
            
        }

        public List<NpcConfiguration> GetConfigurations()
        {
            return _npcConfigurations;
        }

        public void StartConfig()
        {
            StartConfiguration(ConfigurationType.Npc);
        }
    }
}