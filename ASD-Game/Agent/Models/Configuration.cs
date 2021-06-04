using System;
using System.Collections.Generic;
using System.Linq;

namespace Agent.Models
{
    public abstract class Configuration
    {
        private List<KeyValuePair<string, string>> _settings;

        public List<KeyValuePair<string, string>> Settings
        {
            get => _settings;
            set => _settings = value;
        }

        public string GetSetting(string setting)
        {
            return _settings.Where(x => x.Key == setting).FirstOrDefault().Value;
        }
    }
}