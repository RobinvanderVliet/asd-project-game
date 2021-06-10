using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ASD_Game.Session.DTO;

namespace ASD_Game.UserInterface
{
    public class SessionScreen : Screen
    {
        private const int SESSIONS_X = 0;
        private const int SESSIONS_Y = HEADER_HEIGHT + BORDER_SIZE;
        private const int SESSIONS_WIDTH = SCREEN_WIDTH - BORDER_SIZE;

        private const int INPUT_X = 0;
        private const int INPUT_Y = SESSIONS_Y + BORDER_SIZE;
        private const string INPUT_MESSAGE = "Insert session number and username to join";

        private List<SessionDTO> _sessionsInfoList = new();

        public List<SessionDTO> SessionInfoList
        {
            set => _sessionsInfoList = value;
        }

        public override void DrawScreen()
        {
            DrawHeader(GetHeaderText());
            DrawSessionBox();
            UpdateInputMessage(INPUT_MESSAGE);
        }

        public void UpdateWithNewSession(List<SessionDTO> sessions)
        {
            _screenHandler.ConsoleHelper.ClearConsole();
            _sessionsInfoList = sessions;
            DrawHeader(GetHeaderText());
            DrawSessionBox();
            UpdateInputMessage(INPUT_MESSAGE);
        }

        private void DrawSessionBox()
        {
            foreach (var session in _sessionsInfoList)
            {
                // Created by: " + session.Clients[0][1] + " | Players: " + session.Clients.Count + "/8"
                int position = _sessionsInfoList.IndexOf(session);
                _screenHandler.ConsoleHelper.SetCursor(SESSIONS_X + 1, SESSIONS_Y + position);
                _screenHandler.ConsoleHelper.SetCursor(SESSIONS_X + BORDER_SIZE / 2, SESSIONS_Y + OFFSET_TOP + position);
                _screenHandler.ConsoleHelper.Write(" ");
                _screenHandler.ConsoleHelper.SetCursor(SESSIONS_X + OFFSET_LEFT, SESSIONS_Y + OFFSET_TOP + position);
                _screenHandler.ConsoleHelper.Write("(" + (position + 1) + ") " + session.Name + " | Status: " + (session.SessionStarted ? "In progress" : "Not started"));
                _screenHandler.ConsoleHelper.Write(new string(' ', SCREEN_WIDTH - _screenHandler.ConsoleHelper.GetCursorLeft() - BORDER_SIZE / 2));
            }

            DrawBox(SESSIONS_X, SESSIONS_Y, SESSIONS_WIDTH, _sessionsInfoList.Count);
        }

        private string GetHeaderText()
        {
            return "There are currently " + _sessionsInfoList.Count + " sessions you can join";
        }

        public virtual string GetSessionIdByVisualNumber(int sessionNumber)
        {
            if (_sessionsInfoList.ElementAtOrDefault(sessionNumber) != null)
            {
                return _sessionsInfoList.ElementAt(sessionNumber).SessionId;
            }

            return null;
        }

        public virtual void UpdateInputMessage(string message)
        {
            DrawInputBox(INPUT_X, INPUT_Y + _sessionsInfoList.Count, message);
        }
    }
}