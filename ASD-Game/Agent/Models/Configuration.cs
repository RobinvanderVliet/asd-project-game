using System;
using System.Collections.Generic;
using System.Linq;

namespace Agent.Models
{
    public abstract class Configuration
    {
        private List<ValueTuple<string, string>> _settings;

        public List<ValueTuple<string, string>> Settings
        {
            get => _settings;
            set => _settings = value;
        }

        public string GetSetting(string setting)
        {
            return _settings.Where(x => x.Item1 == setting).FirstOrDefault().Item2;
        }
    }
}