using System.Collections.Generic;
using ASD_Game.Agent.Mapper;
using ASD_Game.InputHandling;
using Configuration = ASD_Game.Agent.Models.Configuration;


namespace ASD_Game.Agent.Services
{
    public abstract class BaseConfigurationService
    {
        public FileToSettingListMapper FileToSettingListMapper;

        private FileHandler _fileHandler;
        private Pipeline _pipeline;

        public Pipeline Pipeline { get => _pipeline; set => _pipeline = value; }
        public FileHandler FileHandler { get => _fileHandler; set => _fileHandler = value; }

        public InputHandler InputHandler;
        
        protected const string CANCEL_COMMAND = "cancel";
        protected const string LOAD_COMMAND = "load";
        protected const string EDITOR_COMMAND = "editor";
        public string LastError = "";

        public abstract void CreateConfiguration(string configurationName, string filepath);

        public abstract List<Configuration> GetConfigurations();

    }
}