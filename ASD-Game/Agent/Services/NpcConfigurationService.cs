using ASD_Game.Agent.Mapper;
using ASD_Game.Agent.Models;
using System.Collections.Generic;

namespace ASD_Game.Agent.Services
{
    public class NpcConfigurationService : BaseConfigurationService
    {
        private readonly List<Configuration> _npcConfigurations;

        public NpcConfigurationService(List<Configuration> npcConfigurations, FileToDictionaryMapper fileToDictionaryMapper)
        {
            _npcConfigurations = npcConfigurations;
            FileToDictionaryMapper = fileToDictionaryMapper;
            FileHandler = new FileHandler();
            Pipeline = new Pipeline();
        }

        public override void CreateConfiguration(string configurationName, string filepath)
        {
            var npcConfiguration = new NpcConfiguration();
            npcConfiguration.NpcName = configurationName;
            npcConfiguration.Settings = FileToDictionaryMapper.MapFileToConfiguration(filepath);
            _npcConfigurations.Add(npcConfiguration);
        }

        public override List<Configuration> GetConfigurations()
        {
            return _npcConfigurations;
        }

        public override void Configure()
        {
            
        }
    }
}