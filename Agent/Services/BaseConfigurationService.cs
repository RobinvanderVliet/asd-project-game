using System;
using System.Collections.Generic;
using Agent.exceptions;
using Agent.Mapper;
using Agent.Models;
using Configuration = Agent.Models.Configuration;

namespace Agent.Services
{
    public abstract class BaseConfigurationService
    {
        public FileToDictionaryMapper FileToDictionaryMapper;

        private FileHandler _fileHandler;
        private Pipeline _pipeline;
        
        public Pipeline Pipeline { get => _pipeline; set => _pipeline = value; }
        public FileHandler FileHandler { get => _fileHandler; set => _fileHandler = value; }

        public ConsoleRetriever ConsoleRetriever;
        
        protected const string CANCEL_COMMAND = "cancel";
        protected const string LOAD_COMMAND = "load";
        public string LastError = "";
        
        public abstract void CreateConfiguration(string configurationName, string filepath);

        public abstract List<Configuration> GetConfigurations();

        public abstract void Configure();
    }
}