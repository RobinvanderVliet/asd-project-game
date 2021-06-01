using System;
using System.Collections.Generic;
using System.Linq;

namespace UserInterface
{
    public class SessionScreen : Screen
    {
        private const int SESSIONS_X = 0;
        private const int SESSIONS_Y = HEADER_HEIGHT + BORDER_SIZE;
        private const int SESSIONS_WIDTH = SCREEN_WIDTH - BORDER_SIZE;
        
        private const int INPUT_X = 0;
        private const int INPUT_Y = SESSIONS_Y + BORDER_SIZE;
        private const string INPUT_MESSAGE = "Insert session number and username to join";

        private List<string[]> _sessionsInfoList = new();
        public List<string[]> SessionInfoList
        {
            get => _sessionsInfoList;
            set => _sessionsInfoList = value;
        }
        public override void DrawScreen()
        {
            DrawHeader(GetHeaderText());
            DrawSessionBox();
            UpdateInputMessage(INPUT_MESSAGE);
        }
        
        public void UpdateWithNewSession(string[] sessionInfo)
        {
            _sessionsInfoList.Add(sessionInfo);
            DrawHeader(GetHeaderText());
            DrawSessionBox();
            UpdateInputMessage(INPUT_MESSAGE);
        }
        private void DrawSessionBox()
        {
            foreach (var session in _sessionsInfoList)
            {
                int position = _sessionsInfoList.IndexOf(session);
                Console.SetCursorPosition(SESSIONS_X + 1, SESSIONS_Y + position);
                Console.SetCursorPosition(SESSIONS_X + BORDER_SIZE / 2, SESSIONS_Y + OFFSET_TOP + position);
                Console.Write(" ");
                Console.SetCursorPosition(SESSIONS_X + OFFSET_LEFT, SESSIONS_Y + OFFSET_TOP + position);
                Console.Write("(" + (position + 1) + ") " + session[1] + " | Created by: " + session[2] + " | Players: " + session[3] + "/8");
                Console.Write(new string(' ', SCREEN_WIDTH - Console.CursorLeft - BORDER_SIZE / 2));
            }
            
            DrawBox(SESSIONS_X, SESSIONS_Y, SESSIONS_WIDTH, _sessionsInfoList.Count);
        }

        private string GetHeaderText()
        {
            return "There are currently " + _sessionsInfoList.Count + " sessions you can join";
        }

        public string GetSessionIdByVisualNumber(int sessionNumber)
        {
            if(_sessionsInfoList.ElementAtOrDefault(sessionNumber) != null)
            {
                return _sessionsInfoList[sessionNumber][0];
            }

            return null;
        }

        public void UpdateInputMessage(string message)
        {
            DrawInputBox(INPUT_X, INPUT_Y + _sessionsInfoList.Count, message);
        }
    }
}