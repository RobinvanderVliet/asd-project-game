using System.Collections.Generic;
using Agent;
using ASD_project.Agent.Mapper;
using ASD_project.InputHandling;
using InputHandling;
using Configuration = ASD_project.Agent.Models.Configuration;

namespace ASD_project.Agent.Services
{
    public abstract class BaseConfigurationService
    {
        public FileToDictionaryMapper FileToDictionaryMapper;

        private FileHandler _fileHandler;
        private Pipeline _pipeline;

        public Pipeline Pipeline { get => _pipeline; set => _pipeline = value; }
        public FileHandler FileHandler { get => _fileHandler; set => _fileHandler = value; }

        public InputHandler InputHandler;
        
        protected const string CANCEL_COMMAND = "cancel";
        protected const string LOAD_COMMAND = "load";
        public string LastError = "";

        public abstract void CreateConfiguration(string configurationName, string filepath);

        public abstract List<Configuration> GetConfigurations();

        public abstract void Configure();
    }
}