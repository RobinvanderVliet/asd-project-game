namespace Session.GameConfiguration
{
    public interface IGameConfigurationHandler
    {
        MonsterDifficulty NewMonsterDifficulty { get; set; }
        MonsterDifficulty CurrentMonsterDifficulty { get; set; }
        void HandleAnswer(string input);

        void SetGameConfiguration();

        void SetDifficulty(MonsterDifficulty monsterDifficulty, string sessionId);
    }
}