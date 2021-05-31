using System;

namespace UserInterface
{
    public class GameScreen : Screen
    {
        private GameStatScreen _gameStatScreen;
        private const int STAT_HEIGHT = 5;

        private const int CHAT_X = 0;
        private const int CHAT_Y = STAT_HEIGHT + 2;
        private const int CHAT_WIDTH = (SCREEN_WIDTH - 2) / 2;
        private const int CHAT_HEIGHT = 10;

        private const int WORLD_X = CHAT_WIDTH + 2;
        private const int WORLD_Y = STAT_HEIGHT + 2;
        private const int WORLD_WITH = (SCREEN_WIDTH - 6) / 2;
        private const int WORLD_HEIGHT = 10;

        private const int INPUT_X = 0;
        private const int INPUT_Y = STAT_HEIGHT + CHAT_HEIGHT + 4;

        public GameScreen()
        {
            _gameStatScreen = new GameStatScreen(STAT_HEIGHT);
        }

        public override void DrawScreen()
        {
            _gameStatScreen.DrawScreen();
            DrawChatBox();
            DrawWorldBox();
            DrawInputBox(INPUT_X, INPUT_Y, "Insert an option");
        }

        public void DrawChatBox()
        {
            DrawBox(CHAT_X, CHAT_Y, CHAT_WIDTH, CHAT_HEIGHT);
        }

        public void DrawWorldBox()
        {
            DrawBox(WORLD_X, WORLD_Y, WORLD_WITH, WORLD_HEIGHT);
        }

        
    }
}