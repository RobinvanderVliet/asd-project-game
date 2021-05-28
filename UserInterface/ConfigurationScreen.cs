using System;

namespace UserInterface
{
    public class ConfigurationScreen : Screen
    {
        private const int CONFIGURATION_X = 1;
        private const int CONFIGURATION_Y = HEADER_HEIGHT + BORDER_SIZE;
        private const int CONFIGURATION_WIDTH = SCREEN_WIDTH - BORDER_SIZE;
        
        private const int INPUT_X = 0;
        private const int INPUT_Y = HEADER_HEIGHT + CONFIGURATION_Y;
        private IGameConfigurationHandler _gameConfigurationHandler;

        public ConfigurationScreen()
        {
            _gameConfiguration = new GameConfiguration.GameConfiguration();
        }
        public override void DrawScreen()
        {
            DrawHeader(GetHeaderText());
            DrawConfigurationBox();
            DrawInputBox(INPUT_X, INPUT_Y + 6, "Vul je keuze in");
        }
        
        private string GetHeaderText()
        {
            return "Welcome to the game configuration!";
        }

        private void DrawConfigurationBox()
        {
            foreach (var configuration in _gameConfiguration.ConfigurationHeader)
            {
                int position = _gameConfiguration.ConfigurationHeader.IndexOf(configuration);
                Console.SetCursorPosition(CONFIGURATION_X, CONFIGURATION_Y + position);
                Console.SetCursorPosition(CONFIGURATION_X + OFFSET_LEFT, CONFIGURATION_Y + OFFSET_TOP + position);
                Console.Write(configuration);
                
                foreach (var option in _gameConfiguration.ConfigurationChoices[position])
                {
                    int choicePosition = position + _gameConfiguration.ConfigurationChoices[position].IndexOf(option) + 1;
                    Console.SetCursorPosition(CONFIGURATION_X, CONFIGURATION_Y + choicePosition);
                    Console.SetCursorPosition(CONFIGURATION_X + OFFSET_LEFT, CONFIGURATION_Y + OFFSET_TOP + choicePosition);
                    Console.Write( _gameConfiguration.ConfigurationChoices[position].IndexOf(option) + 1 + ". " + option);
                }
            }
            DrawBox(CONFIGURATION_X - 1, CONFIGURATION_Y, CONFIGURATION_WIDTH, 5);
        }
    }
}