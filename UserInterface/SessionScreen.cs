using System;
using Session;

namespace UserInterface
{
    public class SessionScreen : Screen
    {
        private const int SESSIONS_X = 0;
        private const int SESSIONS_Y = HEADER_HEIGHT + 2;
        private const int SESSIONS_WIDTH = SCREEN_WIDTH - 2;
        
        private const int INPUT_X = 0;
        private const int INPUT_Y = HEADER_HEIGHT + SESSIONS_Y + 4;

        private ISessionHandler _sessionHandler;
        public SessionScreen(ISessionHandler sessionHandler)
        {
            _sessionHandler = sessionHandler;
            _sessionHandler.RequestSessions();
        }
        
        public override void DrawScreen()
        {
            DrawHeader("Showing x sessions");
            DrawSessionBox();
            DrawInputBox(INPUT_X, INPUT_Y, "Insert session ID");
        }

        private void DrawSessionBox()
        {
            
            DrawBox(SESSIONS_X, SESSIONS_Y, SESSIONS_WIDTH, 3);
        }

        public override void HandleInput()
        {
     
        }
    }
}