using System;
using System.Collections.Generic;

namespace UserInterface
{
    public class ConfigurationScreen : Screen
    {
        private string configuration;
        private List<string> options;
        private string inputText;
        private const int CONFIGURATION_X = 1;
        private const int CONFIGURATION_Y = HEADER_HEIGHT + BORDER_SIZE;
        private const int CONFIGURATION_WIDTH = SCREEN_WIDTH - BORDER_SIZE;

        private const int INPUT_X = 0;
        private const int INPUT_Y = HEADER_HEIGHT + CONFIGURATION_Y + 2;

        public ConfigurationScreen()
        {
            configuration = "Do you want to configure the game?";
            options = new List<string>()
            {
                "Yes", "No"
            };
            inputText = "Choose an option";
        }
        public override void DrawScreen()
        {
            Console.Clear();
            DrawHeader(GetHeaderText());
            DrawConfigurationBox();
            DrawInputBox(INPUT_X, INPUT_Y + options.Count, inputText);
        }

        public void UpdateConfigurationScreen(string configurationHeader, List<string> configurationChoices)
        {
            configuration = configurationHeader;
            options = configurationChoices;
            DrawScreen();
        }

        private string GetHeaderText()
        {
            return "Welcome to the game configuration!";
        }

        private void DrawConfigurationBox()
        {
            Console.SetCursorPosition(CONFIGURATION_X, CONFIGURATION_Y);
            Console.SetCursorPosition(CONFIGURATION_X + OFFSET_LEFT, CONFIGURATION_Y + OFFSET_TOP);
            Console.Write(configuration);

            foreach (var option in options)
            {
                int choicePosition = options.IndexOf(option) + 1;
                Console.SetCursorPosition(CONFIGURATION_X + 1, CONFIGURATION_Y + choicePosition);
                Console.SetCursorPosition(CONFIGURATION_X + OFFSET_LEFT, CONFIGURATION_Y + OFFSET_TOP + choicePosition);
                Console.Write("{0}. {1}", choicePosition, option);
            }

            DrawBox(CONFIGURATION_X - 1, CONFIGURATION_Y, CONFIGURATION_WIDTH, options.Count + 1);
        }

        public void UpdateInputMessage(string message)
        {
            inputText = message;
            DrawScreen();
        }
    }
}