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
        private const string CancelCommand = "cancel";
        private const string LOADCOMMAND = "load";
        public ConsoleRetriever consoleRetriever;
        //This is needed for tests, dont delete!
        public String testVar = "";
        private InlineConfig inlineConfig;

        public AgentConfigurationService()
        {
            _pipeline = new Pipeline();
            _fileHandler = new FileHandler();
            consoleRetriever = new ConsoleRetriever();
            inlineConfig = new InlineConfig();
        }
        
        public void StartConfiguration()
        {
            Console.WriteLine("Please provide a path to your code file");
            var input = consoleRetriever.GetConsoleLine();

            if (input.Equals(CancelCommand))
            {
                return;
            }
            
            if (input.Equals(LOADCOMMAND)) 
            {
                inlineConfig.setup();
                return;
            }

            var content = String.Empty;;
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