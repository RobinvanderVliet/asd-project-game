using System.Collections.Generic;
using System.Linq;
using ASD_Game.Agent.Mapper;

// using ASD_Game.Agent.Antlr.Ast;

namespace ASD_Game.Agent.Models
{
    public abstract class Configuration
    {
        private List<Setting> _settings;

        public List<Setting> Settings
        {
            get => _settings;
            set => _settings = value;
        }
        
        public string GetSetting(string settingkey)
        {
            return _settings.Where(x=>x.Property == settingkey).FirstOrDefault().Value;
        }
    }


}