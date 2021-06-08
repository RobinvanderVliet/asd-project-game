using Agent.Models;

namespace Agent.Services
{
    public interface IConfigurationService
    {
        void CreateConfiguration(string name);

        public Configuration Configuration { get; set; }
    }
}