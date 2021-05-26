using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Agent.Models
{
    public abstract class Configuration
    {
        protected List<string[]> _settings;

        public List<string[]> Settings
        {
            get => _settings;
            set => _settings = value;
        }
        
        public string GetSetting(string setting)
        {
            return _settings.Where(x => x.Contains(setting)).FirstOrDefault().ToString();
        }
    }
    
    
}