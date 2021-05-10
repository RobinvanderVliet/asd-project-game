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
        public FileToDictionaryMapper _fileToDictionaryMapper;
        public FileHandler _fileHandler { get; set; }
        public Pipeline _pipeline { get; set; }
        public abstract void CreateConfiguration(string configurationName, string filepath);

        public abstract List<Configuration> GetConfigurations();

        public abstract void StartConfiguration(ConfigurationType configurationType);
    }
}