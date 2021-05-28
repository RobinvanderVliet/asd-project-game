using System;
using System.Collections.Generic;

namespace UserInterface
{
    public class EditorScreen : Screen
    {
 
        private string _displayedQuestions;
        
        public override void DrawScreen()
        {
            DrawHeader("Editor!");
            DrawBox(0, 3, SCREEN_WIDTH - BORDER_SIZE, 4);
            Console.SetCursorPosition(2, 4);
            Console.WriteLine(_displayedQuestions);
            DrawInputBox(0, 10, "Enter an answer");
        }

        public void UpdateLastQuestion(string question)
        {
            _displayedQuestions = question;
            DrawScreen();
        }

        public void PrintWarning(string warning)
        {
            Console.SetCursorPosition(2, 6);
            Console.WriteLine(warning);
        }
    }
}