using System.Collections.Generic;

namespace Agent.Models
{
    public class AgentConfiguration : Configuration
    {
        private string _agentName;

        public string AgentName
        {
            get => _agentName;
            set => _agentName = value;
        }
        
        private Dictionary<string, string> _settings = new Dictionary<string, string>();

        //Function to get full dictionary
        public Dictionary<string, string> Settings
        {
            get => _settings;
            set => _settings = value;
        }

        //Function to get a single setting from dictionary
        public string GetSetting(string setting)
        {
            return this._settings[setting];
        }

        public void SetSetting(string key, string value)
        {
            this._settings.Add(key, value);
        }
    }
}