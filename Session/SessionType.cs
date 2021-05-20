namespace Session
{
    public enum SessionType
    {
        RequestSessions,
        RequestSessionsResponse,
        RequestToJoinSession,
        ClientJoinedSession,
        SendPing,
        ReceivedPingResponse,
        SendHeartbeat
    }
}