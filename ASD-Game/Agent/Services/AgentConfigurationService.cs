using Agent.Exceptions;
using Agent.Mapper;
using Agent.Models;
using InputHandling;
using Serilog;
using System;
using System.Collections.Generic;

namespace Agent.Services
{
    public class AgentConfigurationService : BaseConfigurationService
    {
        private readonly InlineConfig _inlineConfig;
        private readonly List<Configuration> _agentConfigurations;
        private readonly InputHandler _inputHandler;

        public AgentConfigurationService(List<Configuration> agentConfigurations, FileToDictionaryMapper fileToDictionaryMapper, InputHandler inputHandler)
        {
            FileToDictionaryMapper = fileToDictionaryMapper;
            _agentConfigurations = agentConfigurations;
            _inputHandler = inputHandler;
            _inlineConfig = new InlineConfig();
            FileHandler = new FileHandler();
            Pipeline = new Pipeline();
        }

        public override void Configure()
        {
            Console.WriteLine("Please provide a path to your code file");
            var input = _inputHandler.GetCommand();

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

        public override void CreateConfiguration(string configurationName, string filepath)
        {
            var agentConfiguration = new AgentConfiguration();
            agentConfiguration.AgentName = configurationName;
            agentConfiguration.Settings = FileToDictionaryMapper.MapFileToConfiguration(filepath);
            _agentConfigurations.Add(agentConfiguration);
        }

        public override List<Configuration> GetConfigurations()
        {
            return _agentConfigurations;
        }

    }


}