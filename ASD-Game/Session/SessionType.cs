namespace ASD_Game.Session
{
    public enum SessionType
    {
        RequestSessions,
        RequestSessionsResponse,
        RequestToJoinSession,
        SendPing,
        ReceivedPingResponse,
        SendHeartbeat,
        SendAgentConfiguration,
        EditMonsterDifficulty,
        EditItemSpawnRate,
        NewBackUpHost
    }
}