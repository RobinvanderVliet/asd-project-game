using System.Collections.Generic;
using Agent.Mapper;
using Configuration = Agent.Models.Configuration;

namespace Agent.Services
{
    public abstract class BaseConfigurationService
    {
        public FileToSettingListMapper FileToSettingListMapper;

        private FileHandler _fileHandler;
        private Pipeline _pipeline;

        public Pipeline Pipeline { get => _pipeline; set => _pipeline = value; }
        public FileHandler FileHandler { get => _fileHandler; set => _fileHandler = value; }
        
        protected const string CANCEL_COMMAND = "cancel";
        protected const string LOAD_COMMAND = "load";
        protected const string EDITOR_COMMAND = "editor";
        public string LastError = "";
        
        public abstract void CreateConfiguration(string configurationName, string filepath);

        public abstract List<Configuration> GetConfigurations();
    }
}