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

        private List<string> _options = new() {"Host a new session", "Join a session", "Agent editor", "Help", "Leave game"};
        
        private ISessionHandler _sessionHandler;

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

        public override void HandleInput()
        {
            var input = Console.ReadLine();
            int option;
            int.TryParse(input, out option);
            HandleOption(option);
        }

        private void HandleOption(int option)
        {
            switch (option)
            {
                case 1:
                    _screenHandler.TransitionTo(new ConfigurationScreen());
                    _screenHandler.AcceptInput();
                    break;
                case 2:
                    _screenHandler.TransitionTo(new SessionScreen(_sessionHandler));
                    _screenHandler.AcceptInput();
                    break;
                case 3:
                    _screenHandler.TransitionTo(new EditorScreen());
                    _screenHandler.AcceptInput();
                    break;
                case 4:
                    _screenHandler.TransitionTo(new HelpScreen());
                    _screenHandler.AcceptInput();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
                default:
                    DrawInputBox(INPUT_X, INPUT_Y, "Not a valid option! Try again.");
                    HandleInput();
                    break;
            }
        }
    }
}