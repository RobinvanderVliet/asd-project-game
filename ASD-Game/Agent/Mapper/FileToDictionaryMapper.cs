using System;
using System.Collections.Generic;
using System.Data;

namespace ASD_Game.Agent.Mapper
{
    public class FileToDictionaryMapper
    {
        private FileHandler _FileHandler;
        public FileHandler FileHandler { get => _FileHandler; set => _FileHandler = value; }
        public Dictionary<string, string> MapFileToConfiguration(string filepath)
        {
            _FileHandler = new FileHandler();
            Dictionary<string, string> configuration = new Dictionary<string, string>();

            string content = _FileHandler.ImportFile(filepath);

            var splitContent = content.Split(Environment.NewLine);

            foreach (string setting in splitContent)
            {
                if (string.IsNullOrEmpty(setting))
                {
                    throw new SyntaxErrorException("The config file for npc or agent contains an empty row. This is not allowed.");
                }
                //Trim removes spaces before and after given string. string 'Less than' will keep its format.
                var seperatedComponents = setting.Split("=");
                configuration.Add(seperatedComponents[0].Trim(), seperatedComponents[1].Trim());
            }

            return configuration;
        }
    }
}