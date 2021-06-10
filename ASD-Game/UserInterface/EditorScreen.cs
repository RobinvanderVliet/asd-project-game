using System;
using System.Collections.Generic;
using System.Threading;

namespace ASD_Game.UserInterface
{
    public class EditorScreen : Screen
    {
        private const int X_START = 0;
        private const int X_VALUE = 2;
        private const int Y_VALUE = 4;
        private string _displayedQuestions;

        public override void DrawScreen()
        {
            Thread.Sleep(50);
            DrawHeader("Editor!");
            Thread.Sleep(50);
            DrawBox(X_START, 3, SCREEN_WIDTH - BORDER_SIZE, Y_VALUE);
            _screenHandler.ConsoleHelper.SetCursor(X_VALUE, Y_VALUE);
            _screenHandler.ConsoleHelper.Write(_displayedQuestions);
            Thread.Sleep(50);
            DrawInputBox(X_START, 10, "Enter an answer");
        }

        public virtual void UpdateLastQuestion(string question)
        {
            _displayedQuestions = question;
            DrawScreen();
        }

        public virtual void PrintWarning(string warning)
        {
            _screenHandler.ConsoleHelper.SetCursor(2, 6);
            _screenHandler.ConsoleHelper.WriteLine(warning);
        }

        public virtual void ClearScreen()
        {
            _screenHandler.ConsoleHelper.ClearConsole();
        }
    }
}