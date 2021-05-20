using System;
using Agent.exceptions;
using Agent.Exceptions;
using Serilog;

namespace Agent.Services
{
    public class AgentConfigurationService
    {
        private Pipeline _pipeline;
        private FileHandler _fileHandler;
        private const string CANCELCOMMAND = "cancel";
        private const string LOADCOMMAND = "load";

        public ConsoleRetriever ConsoleRetriever;

        //This is needed for tests, dont delete!
        public string testVar = "";
        private InlineConfig _inlineConfig;

        public AgentConfigurationService()
        {
            _pipeline = new Pipeline();
            _fileHandler = new FileHandler();
            ConsoleRetriever = new ConsoleRetriever();
            _inlineConfig = new InlineConfig();
        }

        public void StartConfiguration()
        {
            Console.WriteLine("Please provide a path to your code file");
            var input = ConsoleRetriever.GetConsoleLine();

            if (input.Equals(CANCELCOMMAND))
            {
                return;
            }

            if (input.Equals(LOADCOMMAND))
            {
                _inlineConfig.setup();
                return;
            }

            var content = string.Empty;
            ;
            try
            {
                content = _fileHandler.ImportFile(input);
            }
            catch (FileException e)
            {
                Log.Logger.Information(e.Message);
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
                testVar = e.Message;
                Log.Logger.Information("Syntax error: " + e.Message);
                StartConfiguration();
            }
            catch (SemanticErrorException e)
            {
                Log.Logger.Information("Semantic error: " + e.Message);
                StartConfiguration();
            }
        }

    }
}