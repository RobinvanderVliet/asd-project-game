using System;
using System.Collections.Generic;
using System.IO;
using Agent.antlr.ast.comparables.subjects;
using Agent.Mapper;
using Agent.Models;
using Antlr4.Runtime.Misc;

namespace Agent.Services
{
    public class NpcConfigurationService
    {
        private List<NpcConfiguration> _npcConfigurations;
        private FileToDictionaryMapper _fileToDictionaryMapper;

        public NpcConfigurationService(List<NpcConfiguration> npcConfigurations, FileToDictionaryMapper fileToDictionaryMapper)
        {
            _npcConfigurations = npcConfigurations;
            _fileToDictionaryMapper = fileToDictionaryMapper;
        }

        public void CreateNpcConfiguration(string npcname, string filepath)
        {
            var npcConfiguration = new NpcConfiguration();
            npcConfiguration.NpcName = npcname;

            npcConfiguration.Settings = _fileToDictionaryMapper.MapFileToConfiguration(filepath);
            
            _npcConfigurations.Add(npcConfiguration);
            
        }
        
        
        public List<NpcConfiguration> GetConfigurations()
        {
            return _npcConfigurations;
        }
    }
}