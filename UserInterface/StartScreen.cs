using System;
using System.Collections.Generic;
using Session;

namespace UserInterface
{
    public class StartScreen : Screen
    {
        private const int OPTIONS_X = 0;
        private const int OPTIONS_Y = HEADER_HEIGHT + 2;
        private const int OPTIONS_WIDTH = SCREEN_WIDTH - 2;
        private const int OPTIONS_HEIGHT = 5;
        
        private const int INPUT_X = 0;
        private const int INPUT_Y = HEADER_HEIGHT + OPTIONS_HEIGHT + 4;

        private ISessionHandler _sessionHandler;
        
        private List<string> _options = new() {"Host a new session", "Join a session", "Agent editor", "Help", "Leave game"};
        
        public StartScreen(ISessionHandler sessionHandler)
        {
            _sessionHandler = sessionHandler;
        }
        
        public override void DrawScreen()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            DrawHeader("Welcome to The Apocalypse We Wanted! Pick an option to continue...");
            DrawOptionBox();
            DrawInputBox(INPUT_X, INPUT_Y, "Insert an option");
        }
        private void DrawOptionBox()
        {
            DrawBox(OPTIONS_X, OPTIONS_Y, OPTIONS_WIDTH, OPTIONS_HEIGHT);

            foreach (var option in _options)
            {
                int optionNumber = _options.IndexOf(option);
                Console.SetCursorPosition(2, OPTIONS_Y + optionNumber + 1);
                Console.Write(optionNumber + 1 + ": " + option);
            }
        }
    }
}