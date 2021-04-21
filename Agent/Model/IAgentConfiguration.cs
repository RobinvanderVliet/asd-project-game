using System.IO;

namespace Agent.Model
{
    public interface IAgentConfiguration
    {
        void LoadSettings(Stream fileStream);
        string GetSetting(string settingName);
    }
}