namespace Session.GameConfiguration
{
    public interface IGameConfigurationHandler
    {
        MonsterDifficulty NewMonsterDifficulty { get; set; }
        MonsterDifficulty CurrentMonsterDifficulty { get; set; }
        ItemSpawnRate SpawnRate { get; set; }
        void HandleAnswer(string input);

        void SetGameConfiguration();

        void SetDifficulty(MonsterDifficulty monsterDifficulty, string sessionId);
        void SetSpawnRate(ItemSpawnRate spawnRate, string sessionId);
    }
}