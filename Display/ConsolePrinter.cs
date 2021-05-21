using System;

namespace Display
{
    public class ConsolePrinter : IConsolePrinter
    {
        private ConsoleColor _textColor { get; set; }
        private ConsoleColor _backgroundColor { get; set; }
        
        public ConsolePrinter(ConsoleColor textColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            _textColor = textColor;
            _backgroundColor = backgroundColor;
        }
        
        public void PrintText(string text)
        {
            PrintText(text, _textColor, _backgroundColor);
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