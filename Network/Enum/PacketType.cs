using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Network.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PacketType
    {
        Chat,
        Session
    }
}
