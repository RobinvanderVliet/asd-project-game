using System;

namespace UserInterface
{
    public abstract class Screen
    {
        protected const int SCREEN_WIDTH = 80;
        protected const int HEADER_X = 0;
        protected const int HEADER_Y = 0;
        protected const int HEADER_WIDTH = SCREEN_WIDTH - 2;
        protected const int HEADER_HEIGHT = 1;
        
        protected ScreenHandler _screenHandler;
        
        protected string ulCorner = "╔";
        protected string llCorner = "╚";
        protected string urCorner = "╗";
        protected string lrCorner = "╝";
        protected string vertical = "║";
        protected string horizontal = "═";
        public abstract void DrawScreen();
        public void SetScreen(ScreenHandler screenHandler)
        {
            _screenHandler = screenHandler;
        }
        
        public void DrawBox(int x, int y, int innerWidth, int innerHeight)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(ulCorner);
            for (int i = 0; i < innerWidth; i++)
            {
                Console.Write(horizontal);
            }
            Console.Write(urCorner);
            
            for (int i = 0; i < innerHeight; i++)
            {
                Console.SetCursorPosition(x, i + y + 1);
                Console.Write(vertical);
                
                Console.SetCursorPosition(x + innerWidth + 1, i + y + 1);
                Console.Write(vertical);
            }
            
            Console.SetCursorPosition(x, y + innerHeight + 1);
            Console.Write(llCorner);
            for (int i = 0; i < innerWidth; i++)
            {
                Console.Write(horizontal);
            }

            Console.Write(lrCorner);
        }

        public void DrawHeader(string message)
        {
            DrawBox(HEADER_X, HEADER_Y, HEADER_WIDTH, HEADER_HEIGHT);
            
            Console.SetCursorPosition(HEADER_X + 2, HEADER_Y + 1);
            Console.Write(message);
        }
        public void DrawInputBox(int x, int y, string message)
        {
            DrawBox(x, y, SCREEN_WIDTH - 2, 2);
            Console.SetCursorPosition(x + 2, y + 1);
            Console.Write(message);
            Console.SetCursorPosition(x + 2, y + 2);
            Console.Write(">");

            var emptyLine = String.Empty;
            for (int i = 0; i < 75; i++)
            {
                emptyLine += " ";
            }
            Console.WriteLine(emptyLine);
            Console.SetCursorPosition(x + 4, y + 2);
        }
    }
}