namespace Session
{
    public enum SessionType
    {
        RequestSessions,
        RequestSessionsResponse,
        RequestToJoinSession,
        StartSession,
        ClientJoinedSession,
        SendPing,
        ReceivedPingResponse
    }
}