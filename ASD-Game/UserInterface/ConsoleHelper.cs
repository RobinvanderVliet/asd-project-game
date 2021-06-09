using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.UserInterface
{
    [ExcludeFromCodeCoverage]
    public class ConsoleHelper
    {
        public virtual void ClearConsole()
        {
            Console.Clear();
        }
        
        public virtual void SetColorToGreen()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }

        public virtual void SetCursor(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public virtual void Write(string text)
        {
            Console.Write(text);
        }

        public virtual int GetCursorLeft()
        {
            return Console.CursorLeft;
        }
        
        public virtual int GetCursorTop()
        {
            return Console.CursorTop;
        }
        
        public virtual void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
        
        public virtual string ReadLine()
        {
            return Console.ReadLine();
        }
        
        public virtual int GetConsoleHeight()
        {
            return Console.WindowHeight;
        }

        public virtual int GetConsoleWidth()
        {
            return Console.WindowWidth;
        }
    }
}