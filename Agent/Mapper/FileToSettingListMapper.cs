using System;
using System.Collections.Generic;
using System.Data;

namespace Agent.Mapper
{
    public class FileToSettingListMapper
    {
        public List<Setting> MapFileToConfiguration(string filepath)
        {
            FileHandler fileHandler = new FileHandler();
            List<Setting> configuration = new  List<Setting>();
                    
            string content = fileHandler.ImportFile(filepath);
                    
            var splitContent = content.Split(Environment.NewLine);
                    
            foreach (var setting in splitContent)
            {
                if (setting.Equals(""))
                {
                    throw new SyntaxErrorException("The config file for npc or agent contains an empty row. This is not allowed.");
                }
                //Trim removes spaces before and after given string. string 'Less than' will keep its format.
                var seperatedComponents = setting.Split("=");
                configuration.Add(new Setting(seperatedComponents[0].Trim(), seperatedComponents[1].Trim()));
            }
        
            return configuration;
        }
    }
}