using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Network.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SendAction
    {
        ReturnToSender,
        SendToClients,
        Ignore
    }
}