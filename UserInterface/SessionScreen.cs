using System;
using System.Collections.Generic;
using Session.DTO;

namespace UserInterface
{
    public class SessionScreen : Screen
    {
        private const int SESSIONS_X = 0;
        private const int SESSIONS_Y = HEADER_HEIGHT + 2;
        private const int SESSIONS_WIDTH = SCREEN_WIDTH - 2;
        
        private const int INPUT_X = 0;
        private const int INPUT_Y = HEADER_HEIGHT + SESSIONS_Y + 4;

        private List<SessionDTO> _sessions = new();
        public override void DrawScreen()
        {
            DrawHeader("Showing " + _sessions.Count + " sessions");
            DrawSessionBox();
            DrawInputBox(INPUT_X, INPUT_Y, "Insert session ID");
        }

        public void UpdateSessions(SessionDTO sessionDto)
        {
            _sessions.Add(sessionDto);
            DrawHeader("Showing " + _sessions.Count + " sessions");
            DrawSessionBox();
        }
        private void DrawSessionBox()
        {
            foreach (var session in _sessions)
            {
                Console.SetCursorPosition(SESSIONS_X + 2, SESSIONS_Y + 1 + _sessions.IndexOf(session));
                Console.Write(_sessions.IndexOf(session) + ": " + session.Name);
            }
            
            DrawBox(SESSIONS_X, SESSIONS_Y, SESSIONS_WIDTH, _sessions.Count);
        }
    }
}