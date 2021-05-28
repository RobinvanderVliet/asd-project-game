using System;
using System.Collections.Generic;
using Agent.Mapper;
using Agent.Models;
using Agent.Exceptions;
using Serilog;

namespace Agent.Services
{
    public class AgentConfigurationService : BaseConfigurationService
    {
        private InlineConfig _inlineConfig;
        private List<Configuration> _agentConfigurations;

        public AgentConfigurationService(List<Configuration> agentConfigurations, FileToDictionaryMapper fileToDictionaryMapper)
        {
            FileToDictionaryMapper = fileToDictionaryMapper;
            _agentConfigurations = agentConfigurations;
            _inlineConfig = new InlineConfig();
            FileHandler = new FileHandler();
            Pipeline = new Pipeline();
  
        }
        
        public override void Configure()
        {
            Console.WriteLine("Please provide a path to your code file or type \'editor\' to set agent rules step by step");
            var input = Console.ReadLine();

            if (input.Equals(CANCEL_COMMAND))
            {
                return;
            }
            
            if (input.Equals(LOAD_COMMAND)) 
            {
                _inlineConfig.setup();
                return;
            }

            try
            {
                var content = FileHandler.ImportFile(input);
                Pipeline.ParseString(content);
                Pipeline.CheckAst();
                var output = Pipeline.GenerateAst();

                string fileName = "agent/agent-config.cfg";
                FileHandler.ExportFile(output, fileName);
            }
            catch (SyntaxErrorException e)
            {
                LastError = e.Message;
                Log.Logger.Information("Syntax error: " + e.Message);
                Configure();
            }
            catch (SemanticErrorException e)
            {
                LastError = e.Message;
                Log.Logger.Information("Semantic error: " + e.Message);
                Configure();
            }
            catch (FileException e)
            {
                LastError = e.Message;
                Log.Logger.Information("File error: " + e.Message);
                Configure();
            }
        }
        
        public override void CreateConfiguration(string agentName, string filepath)
        {
            var agentConfiguration = new AgentConfiguration();
            agentConfiguration.AgentName = agentName;
            agentConfiguration.Settings = FileToDictionaryMapper.MapFileToConfiguration(filepath);
            _agentConfigurations.Add(agentConfiguration);
        }
        
        public override List<Configuration> GetConfigurations()
        {
            return _agentConfigurations;
        }
        
    }

    
}