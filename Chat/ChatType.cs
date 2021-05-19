using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Chat
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChatType
    {
        Say,
        Shout,
    }
}
