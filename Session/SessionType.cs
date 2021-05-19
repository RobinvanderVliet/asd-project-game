using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Session
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SessionType
    {
        RequestSessions,
        RequestSessionsResponse,
        RequestToJoinSession
    }
}