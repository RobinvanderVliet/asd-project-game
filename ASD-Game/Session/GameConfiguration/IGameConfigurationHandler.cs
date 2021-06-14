using ASD_Game.Items.Services;

namespace ASD_Game.Session.GameConfiguration
{
    public interface IGameConfigurationHandler
    {
        IItemService ItemService { get; set; }
        MonsterDifficulty GetNewMonsterDifficulty();
        MonsterDifficulty GetCurrentMonsterDifficulty();
        ItemSpawnRate GetItemSpawnRate();
        string GetUsername();
        string GetSessionName();
        bool HandleAnswer(string input);
        void SetGameConfiguration();
        void SetDifficulty(MonsterDifficulty monsterDifficulty, string sessionId);
        void SetSpawnRate(ItemSpawnRate spawnRate, string sessionId);
        void SetCurrentScreen();
    }
}