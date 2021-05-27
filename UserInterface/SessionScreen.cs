using System;
using System.Collections.Generic;
using System.Linq;
using Session.DTO;

namespace UserInterface
{
    public class SessionScreen : Screen
    {
        private const int SESSIONS_X = 0;
        private const int SESSIONS_Y = HEADER_HEIGHT + BORDER_SIZE;
        private const int SESSIONS_WIDTH = SCREEN_WIDTH - BORDER_SIZE;
        
        private const int INPUT_X = 0;
        private const int INPUT_Y = HEADER_HEIGHT + SESSIONS_Y;

        private List<String[]> _sessions = new();
        public override void DrawScreen()
        {
            DrawHeader(GetHeaderText());
            DrawSessionBox();
            DrawInputBox(INPUT_X, INPUT_Y + _sessions.Count + 1, "Insert session number to join session");
        }

        public void UpdateSessions(SessionDTO sessionDto, string sessionId)
        {
            bool alreadyInList = _sessions.Any(arr=> arr.First() == sessionId);
            if (!alreadyInList)
            {
                _sessions.Add(new []{sessionId, sessionDto.Name});
            }
            
            Console.Clear();
            DrawScreen();
        }
        private void DrawSessionBox()
        {
            foreach (var session in _sessions)
            {
                int position = _sessions.IndexOf(session);
                Console.SetCursorPosition(SESSIONS_X + 1, SESSIONS_Y + position);
                Console.SetCursorPosition(SESSIONS_X + OFFSET_LEFT, SESSIONS_Y + OFFSET_TOP + _sessions.IndexOf(session));
                Console.Write(position + 1 + ": " + session[1]);
            }
            
            DrawBox(SESSIONS_X, SESSIONS_Y, SESSIONS_WIDTH, _sessions.Count);
        }

        private string GetHeaderText()
        {
            return "There are currently " + _sessions.Count + " sessions you can join";
        }

        public string GetSessionIdByVisualNumber(int sessionNumber)
        {
            if(_sessions.ElementAtOrDefault(sessionNumber) != null)
            {
                return _sessions[sessionNumber][0];
            }

            return null;
        }

        public void UpdateInputMessage(string message)
        {
            DrawInputBox(INPUT_X, INPUT_Y + _sessions.Count + 1, message);
        }
    }
}