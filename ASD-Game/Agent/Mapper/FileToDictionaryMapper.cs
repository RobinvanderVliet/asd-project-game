using System;
using System.Collections.Generic;
using System.Data;

namespace Agent.Mapper
{
    public class FileToDictionaryMapper
    {
        public FileHandler FileHandler;
        public List<KeyValuePair<string, string>> MapFileToConfiguration(string filepath)
        {
            FileHandler = new FileHandler();
            List<KeyValuePair<string, string>> configuration = new List<KeyValuePair<string, string>>();
                    
            string content = FileHandler.ImportFile(filepath);

            var splitContent = content.Split(Environment.NewLine);

            foreach (var setting in splitContent)
            {
                if (setting.Equals(""))
                {
                    throw new SyntaxErrorException("The config file for npc or agent contains an empty row. This is not allowed.");
                }
                // Trim removes spaces before and after given string. string 'Less than' will keep its format.
                var seperatedComponents = setting.Split("=");
                KeyValuePair<string, string> splitSetting = new KeyValuePair<string, string>(seperatedComponents[0].Trim(), seperatedComponents[1].Trim());
                configuration.Add(splitSetting);
            }

            return configuration;
        }
    }
}