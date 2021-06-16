using ASD_Game.Agent.Models;

namespace Agent.Services
{
    public interface IConfigurationService
    {
        public void CreateConfiguration(string name);

        public Configuration Configuration { get; set; }
    }
}