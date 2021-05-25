using System;

namespace UserInterface
{
    public abstract class Screen
    {
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
    }
}