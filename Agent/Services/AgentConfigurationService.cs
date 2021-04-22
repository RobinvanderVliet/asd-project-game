using System;
using Agent.exceptions;

namespace Agent.Services
{
    public class AgentConfigurationService
    {
        private Pipeline _pipeline;
        private FileHandler _fileHandler;
        private const string CancelCommand = "cancel"; 

        public AgentConfigurationService()
        {
            _pipeline = new Pipeline();
            _fileHandler = new FileHandler();
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
    }
}