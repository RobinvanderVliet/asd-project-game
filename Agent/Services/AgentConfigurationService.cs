using System;
using Agent.exceptions;

namespace Agent.Services
{
    public class AgentConfigurationService
    {
        private Pipeline _pipeline;
        private FileHandler _fileHandler;

        public AgentConfigurationService()
        {
            _pipeline = new Pipeline();
            _fileHandler = new FileHandler();
        }
        
        public void StartConfiguration()
        {
            Console.WriteLine("Please provide a path to your code file");
            var path = Console.ReadLine();
            var content = _fileHandler.ImportFile(path);

            try
            {
                _pipeline.ParseString(content);
                _pipeline.CheckAst();
                _pipeline.GenerateAst();
            }
            catch (SyntaxErrorException e)
            {
                Console.WriteLine("Syntax error: " + e.Message);
            } 
            catch (SemanticErrorException e)
            {
                Console.WriteLine("Semantic error: " + e.Message);
            }
            
            
        }
    }
}