using System;
using System.Collections.Generic;
using Agent.exceptions;
using Agent.Mapper;
using Agent.Models;

namespace Agent.Services
{
    public class AgentConfigurationService
    {
        private Pipeline _pipeline;
        private FileHandler _fileHandler;
        private FileToDictionaryMapper _fileToDictionaryMapper;
        private List<AgentConfiguration> _agentConfigurations;
        private const string CancelCommand = "cancel"; 

        public AgentConfigurationService()
        {
            _pipeline = new Pipeline();
            _fileHandler = new FileHandler();
            _fileToDictionaryMapper = new FileToDictionaryMapper();
        }
        
        public void StartConfiguration()
        {
            Console.WriteLine("Please provide a path to your code file");
            var input = Console.ReadLine();

            if (input.Equals(CancelCommand))
            {
                return;
            }

            var content = String.Empty;;
            try
            {
                content = _fileHandler.ImportFile(input);
            }
            catch (FileException e)
            {
                Console.WriteLine("Something went wrong: " + e);    
                StartConfiguration();
            }
            

            try
            {
                _pipeline.ParseString(content);
                _pipeline.CheckAst();
                var output = _pipeline.GenerateAst();
                _fileHandler.ExportFile(output);
            }
            catch (SyntaxErrorException e)
            {
                Console.WriteLine("Syntax error: " + e.Message);
                StartConfiguration();
            } 
            catch (SemanticErrorException e)
            {
                Console.WriteLine("Semantic error: " + e.Message);
                StartConfiguration();
            }
        }

        public void CreateAgentConfiguration(string agentName, string filepath)
        {
            var agentConfiguration = new AgentConfiguration();
            agentConfiguration.AgentName = agentName;

           agentConfiguration.Settings = _fileToDictionaryMapper.MapFileToConfiguration(filepath);

           _agentConfigurations.Add(agentConfiguration);



        }
    }
}