using System;
using Agent.exceptions;
using Agent.Mapper;

namespace Agent.Services
{
    public class BaseConfigurationService
    {
        private Pipeline _pipeline;
        private FileHandler _fileHandler { get; set; }
        private const string Cancelcommand = "cancel";
        private FileToDictionaryMapper _fileToDictionaryMapper;
        
        public void StartConfiguration(ConfigurationType configurationType)
        {
            Console.WriteLine("Please provide a path to your code file");
            var input = Console.ReadLine();

            if (input.Equals(Cancelcommand))
            {
                return;
            }

            var content = String.Empty;
            
            try
            {
                content = _fileHandler.ImportFile(input);
            }
            catch (FileException e)
            {
                Console.WriteLine("Something went wrong: " + e);
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
                Console.WriteLine("Syntax error: " + e.Message);
                StartConfiguration(configurationType);
            }
            catch (SemanticErrorException e)
            {
                Console.WriteLine("Semantic error: " + e.Message);
                StartConfiguration(configurationType);
            }
        }

        public virtual void CreateConfiguration(string name, string filepath)
        {
            
        }
        
        public Pipeline Pipeline
        {
            get => _pipeline;
            set => _pipeline = value;
        }

        public FileHandler FileHandler
        {
            get => _fileHandler;
            set => _fileHandler = value;
        }

        public FileToDictionaryMapper FileToDictionaryMapper
        {
            get => _fileToDictionaryMapper;
            set => _fileToDictionaryMapper = value;
        }
    }
}