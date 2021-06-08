using System;
using System.Collections.Generic;

namespace Agent.Models
{
    public class Configuration
    {
        private string _name;
        private List<KeyValuePair<string, string>> _settings;

        public Configuration(string name)
        {
            _name = name;
        }

        public List<KeyValuePair<string, string>> Settings
        {
            get => _settings;
            set => _settings = value;
        }
    }
}