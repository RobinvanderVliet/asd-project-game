using System;
using System.Collections.Generic;
using System.Data;
using System.Security;
using Agent.Exceptions;
using Agent.Models;
using Serilog;

namespace Agent.Mapper
{
    public class FileToDictionaryMapper
    {
        public List<string[]> MapFileToConfiguration(string filepath)
        {
            FileHandler fileHandler = new FileHandler();
            List<string[]> configuration = new List<string[]>();

            string content = String.Empty;
            try
            {
                content = fileHandler.ImportFile(filepath);
            }
            catch (FileException e)
            {
                Log.Logger.Information(e.Message);
            }
            
            
            //Possible problems with mac/linux users because only \r or \n is used
            var splitContent = content.Split("\r\n");
            
            foreach (var setting in splitContent)
            {
                if (setting.Equals(""))
                {
                    throw new SyntaxErrorException("The config file for npc or agent contains an empty row. This is not allowed.");
                }
                    //Trim removes spaces before and after given string. string 'Less than' will keep its format.
                var seperatedComponents = setting.Split("=");
                configuration.Add(new string[] { seperatedComponents[0].Trim(), seperatedComponents[1].Trim() } );
            }

            return configuration;
}
    }
}