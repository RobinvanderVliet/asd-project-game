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
        public ConsoleRetriever _consoleRetriever;
        public String lastError = "";
        private InlineConfig inlineConfig;
        private List<Configuration> _agentConfigurations;

        public AgentConfigurationService(List<Configuration> agentConfigurations, FileToDictionaryMapper fileToDictionaryMapper)
        {
            _fileToDictionaryMapper = fileToDictionaryMapper;
            _agentConfigurations = agentConfigurations; 
            _consoleRetriever = new ConsoleRetriever();
            inlineConfig = new InlineConfig();
            _fileHandler = new FileHandler();
            _pipeline = new Pipeline();
        }
        
        public override void Configure()
        {
            Console.WriteLine("Please provide a path to your code file");
            var input = _consoleRetriever.GetConsoleLine();

            if (input.Equals(CANCEL_COMMAND))
            {
                return;
            }
            
            if (input.Equals(LOAD_COMMAND)) 
            {
                inlineConfig.setup();
                return;
            }
            
            try
            {
                var content = _fileHandler.ImportFile(input);
                _pipeline.ParseString(content);
                _pipeline.CheckAst();
                var output = _pipeline.GenerateAst();

                string fileName = "agent\\agent-config.cfg";
                _fileHandler.ExportFile(output, fileName);
            }
            catch (SyntaxErrorException e)
            {
                lastError = e.Message;
                Log.Logger.Information("Syntax error: " + e.Message);
                Configure();
            }
            catch (SemanticErrorException e)
            {
                lastError = e.Message;
                Log.Logger.Information("Semantic error: " + e.Message);
                Configure();
            }
            catch (FileException e)
            {
                lastError = e.Message;
                Log.Logger.Information("File error: " + e.Message);
                Configure();
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