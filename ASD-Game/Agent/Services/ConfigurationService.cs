using Agent.Exceptions;
using Agent.Mapper;
using Agent.Models;
using Agent.Exceptions;
using InputHandling;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace Agent.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private InlineConfig _inlineConfig;
        private Configuration _configuration;
        private readonly IFileToConfigurationMapper _fileToConfigurationMapper;
        private FileHandler _fileHandler;
        public FileHandler FileHandler { set => _fileHandler = value; }
        public Configuration Configuration { 
            get => _configuration;
            set => _configuration = value;
        }

        public ConfigurationService(IFileToConfigurationMapper fileToConfigurationMapper)
        {
            _fileToConfigurationMapper = fileToConfigurationMapper;
            _inlineConfig = new InlineConfig();
            _fileHandler = new FileHandler();
            // Pipeline = new Pipeline();
        }
        
        public void CreateConfiguration(string name)
        {
            var configuration = new Configuration(name);
            var configurationFileLocation = String.Empty;
            if (name.Equals("agent"))
            {
                configurationFileLocation = "agent" + Path.DirectorySeparatorChar + name + "-config.cfg";    
            }
            else
            {
                configurationFileLocation = "npc" + Path.DirectorySeparatorChar + name + "-config.cfg";
            }
            
            if (!_fileHandler.FileExist(configurationFileLocation))
            {
                _fileHandler.ExportFile("explore=random" + Environment.NewLine + "combat=offensive", configurationFileLocation);
            }

            configuration.Settings = _fileToConfigurationMapper.MapFileToConfiguration(_fileHandler.GetBaseDirectory() + Path.DirectorySeparatorChar + "resource" + Path.DirectorySeparatorChar + configurationFileLocation);
            Configuration = configuration;
        }

    }


}