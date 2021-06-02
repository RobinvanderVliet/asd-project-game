using System;

namespace Display
{
    public interface IConsolePrinter
    {
        void PrintText(string text);
        void PrintText(string text, ConsoleColor textColor, ConsoleColor backgroundColor);
        void NextLine();
    }
}