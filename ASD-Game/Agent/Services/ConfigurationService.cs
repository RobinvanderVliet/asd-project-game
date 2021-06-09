using Agent.Mapper;
using System;
using System.Collections.Generic;
using System.IO;
using ASD_Game.Agent;
using ASD_Game.Agent.Exceptions;
using ASD_Game.Agent.Models;
using Serilog;

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
        private Pipeline _pipeline;
        public Pipeline Pipeline { get => _pipeline; set => _pipeline = value; }
        public string LastError = "";

        public ConfigurationService()
        {
            FileHandler = new FileHandler();
            Pipeline = new Pipeline();
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
        
        public virtual List<string> Configure(string input)
        {
            try
            {
                Pipeline.ParseString(input);
                if (Pipeline.GetErrors().Count <= 0)
                {
                    Pipeline.CheckAst();
                }
                if (Pipeline.GetErrors().Count > 0)
                {
                    return Pipeline.GetErrors();
                }
            
            
            
                if (Pipeline.GetErrors().Count <= 0)
                {
                    var output = Pipeline.GenerateAst();
                    string fileName = "agent/agent-config.cfg";
            
                    _fileHandler.ExportFile(output, fileName);
                }
            }
            catch (FileException e)
            {
                LastError = e.Message;
                Log.Logger.Information("File error: " + e.Message);
            }
            return Pipeline.GetErrors();
        }
    }
}