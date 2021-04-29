using System.Collections.Generic;
using Antlr4.Runtime.Atn;

namespace Agent.Models
{
    public class NpcConfiguration : Configuration
    {
        private string _npcName;

        public string NpcName
        {
            get => _npcName;
            set => _npcName = value;
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

