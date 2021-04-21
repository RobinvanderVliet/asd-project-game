using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Agent.Model
{
    public class AgentConfiguration
    {
        public Dictionary<string, string> settings;

        public AgentConfiguration()
        {
            settings = new Dictionary<string, string>();
        }

        public void LoadSettings(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Configuration file was not found.");
            }

            foreach (var line in File.ReadLines(filePath))
            {
                var setting = line.Split("=");
                settings.Add(setting[0], setting[1]);
            }
        }

        public string GetSetting(string settingName)
        {
            return settings[settingName];
        }
    }
}