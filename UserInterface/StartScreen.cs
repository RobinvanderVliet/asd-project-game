using System;
using System.Collections.Generic;

namespace UserInterface
{
    public class StartScreen : Screen
    {
        private List<string> options = new List<string>();
        public StartScreen()
        {
            options.Add("Host a new session");
            options.Add("Join a session");
            options.Add("Agent editor");
            options.Add("Leave game");
        }
        public override void DrawScreen()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            DrawBox(0, 0, 78, 7);
            foreach (var option in options)
            {
                Console.SetCursorPosition(2, 1 + options.IndexOf(option));
                Console.WriteLine(options.IndexOf(option) + 1 + ": " + option);
            }
            
            DrawBox(0, 9, 78, 2);
            Console.SetCursorPosition(2, 10);
            Console.Write("Insert a number");
            
            Console.SetCursorPosition(2, 11);
            Console.Write("> ");
        }
    }
}