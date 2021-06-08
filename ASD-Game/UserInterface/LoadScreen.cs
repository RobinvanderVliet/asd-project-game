using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UserInterface
{
    public class LoadScreen : Screen
    {
        private const int SESSIONS_X = 0;
        private const int SESSIONS_Y = HEADER_HEIGHT + BORDER_SIZE;
        private const int SESSIONS_WIDTH = SCREEN_WIDTH - BORDER_SIZE;
        private List<string[]> _sessions;
        
        public List<string[]> Sessions
        {
            get => _sessions;
        }
        
        public LoadScreen()
        {
            _sessions = new List<string[]>();
        }
        
        public override void DrawScreen()
        {
            DrawHeader("Load a saved session");
            
            foreach (var session in _sessions)
            {
                int position = _sessions.IndexOf(session);
                _screenHandler.ConsoleHelper.SetCursor(SESSIONS_X + 1, SESSIONS_Y + position);
                _screenHandler.ConsoleHelper.SetCursor(SESSIONS_X + BORDER_SIZE / 2, SESSIONS_Y + OFFSET_TOP + position);
                _screenHandler.ConsoleHelper.Write(" ");
                _screenHandler.ConsoleHelper.SetCursor(SESSIONS_X + OFFSET_LEFT, SESSIONS_Y + OFFSET_TOP + position);
                _screenHandler.ConsoleHelper.Write("(" + (position + 1) + ") " + session[1]);
                _screenHandler.ConsoleHelper.Write(new string(' ', SCREEN_WIDTH - _screenHandler.ConsoleHelper.GetCursorLeft() - BORDER_SIZE / 2));
            }
            
            DrawBox(0, SESSIONS_Y, SCREEN_WIDTH - BORDER_SIZE, _sessions.Count);
            UpdateInputMessage("Insert number of the session you wish to load");
        }

        public virtual void UpdateSavedSessionsList(List<string[]> sessions)
        {
            _screenHandler.ConsoleHelper.ClearConsole();
            _sessions = sessions;
            DrawScreen();
        }

        public virtual void UpdateInputMessage(string message)
        {
            DrawInputBox(0, SESSIONS_Y + _sessions.Count + BORDER_SIZE, message);
        }

        public virtual string GetSessionByPosition(int sessionNumber)
        {
            if(_sessions.ElementAtOrDefault(sessionNumber) != null)
            { 
                return _sessions[sessionNumber][0];
            }
            
            return null;
        }
    }
}