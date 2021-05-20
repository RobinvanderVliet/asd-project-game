using System;

namespace ConsolePrinter
{
    public class ConsolePrinter : IConsolePrinter
    {
        private ConsoleColor TextColor { get; set; }
        private ConsoleColor BackgroundColor { get; set; }

        public ConsolePrinter(ConsoleColor textColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            TextColor = textColor;
            BackgroundColor = backgroundColor;
        }
        
        public void PrintText(string text)
        {
            PrintText(text, TextColor, BackgroundColor);
        }

        public void PrintText(string text, ConsoleColor textColor, ConsoleColor backgroundColor)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = textColor;
            Console.Write(text);
        }
        
        public void NextLine()
        {
            Console.WriteLine("");
        }
    }
}