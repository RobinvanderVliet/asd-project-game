using System;
using System.Collections.Generic;
using Agent.exceptions;
using Agent.Mapper;
using Agent.Models;
using Agent.Services.interfaces;
using Serilog;
using Configuration = Agent.Models.Configuration;

namespace Agent.Services
{
    public class NpcConfigurationService : BaseConfigurationService
    {
        private const string CANCEL_COMMAND = "cancel";
        private List<Configuration> _npcConfigurations;
        public ConsoleRetriever _consoleRetriever;
        public String lastError = "";
        
        public NpcConfigurationService(List<Configuration> npcConfigurations, FileToDictionaryMapper fileToDictionaryMapper)
        {
            _npcConfigurations = npcConfigurations;
            _fileToDictionaryMapper = fileToDictionaryMapper;
            _consoleRetriever = new ConsoleRetriever();
            _fileHandler = new FileHandler();
            _pipeline = new Pipeline();
        }

        public override void CreateConfiguration(string npcName, string filepath)
        {
            var npcConfiguration = new NpcConfiguration();
            npcConfiguration.NpcName = npcName;

            npcConfiguration.Settings = _fileToDictionaryMapper.MapFileToConfiguration(filepath);
            
            _npcConfigurations.Add(npcConfiguration);
        }

        public override List<Configuration> GetConfigurations()
        {
            return _npcConfigurations;
        }

        public override void Configure()
        {
            //TODO: Seems like duplicate code for now, but must be refactored later to match anticipated feature 'Configure NPC during a game'
            Console.WriteLine("What NPC do you wish to configure?");
            var npc = _consoleRetriever.GetConsoleLine();
            
            if (npc.Equals(CANCEL_COMMAND))
            {
                return;
            }
            
            Console.WriteLine("Please provide code for the NPC");
            var code = _consoleRetriever.GetConsoleLine();
            
            
            
            try
            {
                _pipeline.ParseString(code);
                _pipeline.CheckAst();
                var output = _pipeline.GenerateAst();

                string fileName = "npc\\" + npc + "-config.cfg";
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
        }
    }
}