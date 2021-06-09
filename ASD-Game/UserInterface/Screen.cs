namespace ASD_Game.UserInterface
{
    public abstract class Screen
    {
        protected const int SCREEN_WIDTH = 120;
        protected const int HEADER_X = 0;
        protected const int HEADER_Y = 0;
        protected const int HEADER_WIDTH = SCREEN_WIDTH - 2;
        protected const int HEADER_HEIGHT = 1;
        protected const int BORDER_SIZE = 2;
        protected const int OFFSET_TOP = 1;
        protected const int OFFSET_LEFT = 2;
        protected const int INPUT_HEIGHT = 2;
        
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
            _screenHandler.ConsoleHelper.SetCursor(x, y);
            _screenHandler.ConsoleHelper.Write(ulCorner);
            for (int i = 0; i < innerWidth; i++)
            {
                _screenHandler.ConsoleHelper.Write(horizontal);
            }
            _screenHandler.ConsoleHelper.Write(urCorner);
            
            for (int i = 0; i < innerHeight; i++)
            {
                _screenHandler.ConsoleHelper.SetCursor(x, i + y + 1);
                _screenHandler.ConsoleHelper.Write(vertical);
                
                _screenHandler.ConsoleHelper.SetCursor(x + innerWidth + 1, i + y + 1);
                _screenHandler.ConsoleHelper.Write(vertical);
            }
            
            _screenHandler.ConsoleHelper.SetCursor(x, y + innerHeight + 1);
            _screenHandler.ConsoleHelper.Write(llCorner);
            for (int i = 0; i < innerWidth; i++)
            {
                _screenHandler.ConsoleHelper.Write(horizontal);
            }

            _screenHandler.ConsoleHelper.Write(lrCorner);
        }

        public void DrawHeader(string message)
        {
            DrawBox(HEADER_X, HEADER_Y, HEADER_WIDTH, HEADER_HEIGHT);
            _screenHandler.ConsoleHelper.SetCursor(HEADER_X + OFFSET_LEFT / 2, HEADER_Y + OFFSET_TOP);
            _screenHandler.ConsoleHelper.Write(" ");
            _screenHandler.ConsoleHelper.SetCursor(HEADER_X + OFFSET_LEFT, HEADER_Y + OFFSET_TOP);
            _screenHandler.ConsoleHelper.Write(message);
            _screenHandler.ConsoleHelper.Write(new string(' ', SCREEN_WIDTH - BORDER_SIZE / 2 - _screenHandler.ConsoleHelper.GetCursorLeft()));
        }
        public void DrawInputBox(int x, int y, string message)
        {
            DrawBox(x, y, SCREEN_WIDTH - BORDER_SIZE, INPUT_HEIGHT);
            _screenHandler.ConsoleHelper.SetCursor(x + OFFSET_LEFT - OFFSET_TOP, y + OFFSET_TOP);
            _screenHandler.ConsoleHelper.Write(new string(' ', SCREEN_WIDTH - BORDER_SIZE));
            _screenHandler.ConsoleHelper.SetCursor(x + OFFSET_LEFT, y + OFFSET_TOP);
            _screenHandler.ConsoleHelper.Write(message);
            _screenHandler.ConsoleHelper.SetCursor(x + OFFSET_LEFT / 2, y + BORDER_SIZE);
            _screenHandler.ConsoleHelper.Write(" ");
            _screenHandler.ConsoleHelper.SetCursor(x + OFFSET_LEFT, y + BORDER_SIZE);
            _screenHandler.ConsoleHelper.Write(">");
            _screenHandler.ConsoleHelper.Write(new string(' ', SCREEN_WIDTH - BORDER_SIZE / 2 - _screenHandler.ConsoleHelper.GetCursorLeft()));
            _screenHandler.ConsoleHelper.SetCursor(x + OFFSET_LEFT * 2, y + OFFSET_TOP * 2);
        }
    }
}