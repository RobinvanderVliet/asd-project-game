using System;
using System.Collections.Generic;
using ASD_Game.Agent.Exceptions;
using ASD_Game.Agent.Mapper;
using ASD_Game.Agent.Models;
using Serilog;

namespace ASD_Game.Agent.Services
{
    public class AgentConfigurationService : BaseConfigurationService
    {
        private readonly List<Configuration> _agentConfigurations;

        public AgentConfigurationService(List<Configuration> agentConfigurations, FileToSettingListMapper fileToSettingListMapper)
        {
            FileToSettingListMapper = fileToSettingListMapper;
            _agentConfigurations = agentConfigurations;
            FileHandler = new FileHandler();
            Pipeline = new Pipeline();
        }
        
        public AgentConfigurationService()
        {
            FileHandler = new FileHandler();
            Pipeline = new Pipeline();
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
                    
                    FileHandler.ExportFile(output, fileName);
                    
                    
                }
            }
            catch (FileException e)
            {
                LastError = e.Message;
                Log.Logger.Information("File error: " + e.Message);
            }
            return Pipeline.GetErrors();
        }

        public override void CreateConfiguration(string agentName, string filepath)
        {
            var agentConfiguration = new AgentConfiguration();
            agentConfiguration.AgentName = agentName;
            agentConfiguration.Settings = FileToSettingListMapper.MapFileToConfiguration(filepath);
            _agentConfigurations.Add(agentConfiguration);
        }

        public override List<Configuration> GetConfigurations()
        {
            return _agentConfigurations;
        }

    }


}