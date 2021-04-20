using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Agent.Model
{
    public class AgentConfiguration
    {
        private Dictionary<string, string> settings;
        
        public AgentConfiguration(string filePath)
        {
            LoadSettings(filePath);
        }

        public void LoadSettings(string filePath)
        {
            using (var fs = File.OpenRead(filePath))
            {
                var b = new byte[1024];
                var temp = new UTF8Encoding(true);
                while (fs.Read(b,0,b.Length) > 0)
                {
                    Console.WriteLine(temp.GetString(b));
                    var setting = temp.GetString(b).Split("=");
                    settings[setting[0]] = setting[1];
                }
            }
        }
    }
}