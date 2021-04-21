using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Agent.Model
{
    public class AgentConfiguration : IAgentConfiguration
    {
        public Dictionary<string, string> settings;

        public AgentConfiguration()
        {
            settings = new Dictionary<string, string>();
        }

        public void LoadSettings(Stream fileStream)
        {
            var sr = new StreamReader(fileStream, Encoding.UTF8);

            string line = String.Empty;

            while ((line = sr.ReadLine()) != null)
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