using System.Collections;

namespace UserInterface
{
    public class LoadScreen : Screen
    {
        private const int SESSIONS_X = 0;
        private const int SESSIONS_Y = HEADER_HEIGHT + BORDER_SIZE;
        private const int SESSIONS_WIDTH = SCREEN_WIDTH - BORDER_SIZE;
        public override void DrawScreen()
        {
            DrawHeader("Load a session, bitch");
            DrawInputBox(0, 5 ,"iets");
        }

        public void UpdateSavedSessionsList(IEnumerable sessions)
        {
            _screenHandler.ConsoleHelper.ClearConsole();
            DrawHeader("Load a session, bitch");

            int position = 0;
            foreach (var session in sessions)
            {
                _screenHandler.ConsoleHelper.SetCursor(SESSIONS_X + 1, SESSIONS_Y + position);
                _screenHandler.ConsoleHelper.SetCursor(SESSIONS_X + BORDER_SIZE / 2, SESSIONS_Y + OFFSET_TOP + position);
                _screenHandler.ConsoleHelper.Write(" ");
                _screenHandler.ConsoleHelper.SetCursor(SESSIONS_X + OFFSET_LEFT, SESSIONS_Y + OFFSET_TOP + position);
                _screenHandler.ConsoleHelper.Write(session.ToString());
                _screenHandler.ConsoleHelper.Write(new string(' ', SCREEN_WIDTH - _screenHandler.ConsoleHelper.GetCursorLeft() - BORDER_SIZE / 2));
                position++;
            }
            
            DrawInputBox(0, SESSIONS_Y + position + BORDER_SIZE, "Insert session you want to load, bitch");
        }
    }
}