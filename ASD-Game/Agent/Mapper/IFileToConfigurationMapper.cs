using System.Collections.Generic;

namespace Agent.Mapper
{
    public interface IFileToConfigurationMapper
    {
        List<KeyValuePair<string, string>> MapFileToConfiguration(string filepath);
    }
}