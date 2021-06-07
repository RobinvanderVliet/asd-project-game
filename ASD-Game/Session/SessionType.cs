namespace ASD_project.Session
{
    public enum SessionType
    {
        RequestSessions,
        RequestSessionsResponse,
        RequestToJoinSession,
        SendPing,
        ReceivedPingResponse,
        SendHeartbeat,
        EditMonsterDifficulty,
        EditItemSpawnRate
    }
}