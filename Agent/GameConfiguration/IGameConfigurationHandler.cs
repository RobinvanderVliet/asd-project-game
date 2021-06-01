using UserInterface;

namespace Agent.GameConfiguration
{
    public interface IGameConfigurationHandler
    {
        void HandleAnswer(string input);

        void SetGameConfiguration();

        void SetDifficulty(MonsterDifficulty monsterDifficulty);

        void SetFrequency(ItemFrequency itemFrequency);
    }
}