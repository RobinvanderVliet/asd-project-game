using System;
using System.Collections.Generic;
using Agent.exceptions;
using Agent.Mapper;
using Agent.Models;
using Agent.Exceptions;
using Serilog;

namespace Agent.Services
{
    public class AgentConfigurationService : BaseConfigurationService
    {
        private const string CANCEL_COMMAND = "cancel";
        private const string LOAD_COMMAND = "load";
        public ConsoleRetriever consoleRetriever;
        //This is needed for tests, dont delete!
        public String testVar = "";
        private InlineConfig inlineConfig;
        private List<Configuration> _agentConfigurations;

        public AgentConfigurationService(List<Configuration> agentConfigurations, FileToDictionaryMapper fileToDictionaryMapper)
        {
            _fileToDictionaryMapper = fileToDictionaryMapper;
            _agentConfigurations = agentConfigurations; 
            consoleRetriever = new ConsoleRetriever();
            inlineConfig = new InlineConfig();
            _fileHandler = new FileHandler();
            _pipeline = new Pipeline();
        }
        
        public override void StartConfiguration(ConfigurationType configurationType)
        {
            Console.WriteLine("Please provide a path to your code file");
            var input = consoleRetriever.GetConsoleLine();

            if (input.Equals(CANCEL_COMMAND))
            {
                return;
            }
            
            if (input.Equals(LOAD_COMMAND)) 
            {
                inlineConfig.setup();
                return;
            }

            var content = String.Empty;
            try
            {
                content = _fileHandler.ImportFile(input);
            }
            catch (FileException e)
            {
                Log.Logger.Information(e.Message);    
                StartConfiguration(configurationType);
            }
            

            try
            {
                _pipeline.ParseString(content);
                _pipeline.CheckAst();
                var output = _pipeline.GenerateAst();
                
                string fileName = String.Empty;
                if (configurationType.Equals(ConfigurationType.Agent))
                {
                    fileName = "agent/" + "agent-config.cfg";
                }
                else
                {
                    fileName = "npc/" + "npc-config.cfg";
                }

                _fileHandler.ExportFile(output, fileName);
            }
            catch (SyntaxErrorException e)
            {
                testVar = e.Message;
                Log.Logger.Information("Syntax error: " + e.Message);
                StartConfiguration(configurationType);
            } 
            catch (SemanticErrorException e)
            {
                Log.Logger.Information("Semantic error: " + e.Message);
                StartConfiguration(configurationType);
            }
        }
        
        public override void CreateConfiguration(string agentName, string filepath)
        {
            var agentConfiguration = new AgentConfiguration();
            agentConfiguration.AgentName = agentName;

            agentConfiguration.Settings = _fileToDictionaryMapper.MapFileToConfiguration(filepath);

            _agentConfigurations.Add(agentConfiguration);
        }
        
        public override List<Configuration> GetConfigurations()
        {
            return _agentConfigurations;
        }
        
    }

    
}