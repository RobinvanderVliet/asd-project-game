using System.Collections.Generic;
using ASD_Game.Agent.Mapper;
using ASD_Game.InputHandling;
using Configuration = ASD_Game.Agent.Models.Configuration;


namespace ASD_Game.Agent.Services
{
    public abstract class BaseConfigurationService
    {
        public FileToDictionaryMapper _fileToDictionaryMapper;

        private FileHandler _fileHandler;
        private Pipeline _pipeline;

        public Pipeline Pipeline { get => _pipeline; set => _pipeline = value; }
        public FileHandler FileHandler { get => _fileHandler; set => _fileHandler = value; }
        public FileToDictionaryMapper FileToDictionaryMapper { get => _fileToDictionaryMapper; set => _fileToDictionaryMapper = value; }

        public InputHandler InputHandler;
        
        protected const string CANCEL_COMMAND = "cancel";
        protected const string LOAD_COMMAND = "load";

        private string _lastError = "";

        public string LastError { get => _lastError; set => _lastError = value; }

        public abstract void CreateConfiguration(string configurationName, string filepath);

        public abstract List<Configuration> GetConfigurations();

        public abstract void Configure();
    }
}