using System.Collections.Generic;
using Agent.Models;

namespace Agent.Services.interfaces
{
    public interface INpcConfigurationService
    {
        public void StartConfig();

        public void CreateConfiguration(string npcname, string filepath);

        public List<NpcConfiguration> GetConfigurations();
    }
}