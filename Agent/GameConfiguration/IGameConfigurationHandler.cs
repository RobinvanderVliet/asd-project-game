using UserInterface;

namespace Agent.GameConfiguration
{
    public interface IGameConfigurationHandler
    {
        void HandleAnswer(string input);

        void SetGameConfiguration();
    }
}