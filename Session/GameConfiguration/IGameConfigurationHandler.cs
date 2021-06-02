namespace Session.GameConfiguration
{
    public interface IGameConfigurationHandler
    {
        MonsterDifficulty GetNewMonsterDifficulty();
        MonsterDifficulty GetCurrentMonsterDifficulty();
        ItemSpawnRate GetSpawnRate();
        void HandleAnswer(string input);
        void SetGameConfiguration();
        void SetDifficulty(MonsterDifficulty monsterDifficulty, string sessionId);
        void SetSpawnRate(ItemSpawnRate spawnRate, string sessionId);
    }
}