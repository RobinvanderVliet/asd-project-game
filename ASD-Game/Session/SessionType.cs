namespace Session
{
    public enum SessionType
    {
        RequestSessions,
        RequestSessionsResponse,
        RequestToJoinSession,
        SendPing,
        ReceivedPingResponse,
        SendHeartbeat,
        NewBackUpHost
    }
}