using System;

namespace UserInterface
{
    public class GameScreen : Screen
    {
        private GameStatScreen _gameStatScreen;
        private GameChatScreen _gameChatScreen;
        private GameWorldScreen _gameWorldScreen;

        private const int STAT_X = HEADER_X;
        private const int STAT_Y = HEADER_Y;
        private const int STAT_WIDTH = HEADER_WIDTH;
        private const int STAT_HEIGHT = 5;

        private const int CHAT_X = HEADER_X;
        private const int CHAT_Y = STAT_HEIGHT + BORDER_SIZE;
        private const int CHAT_WIDTH = (SCREEN_WIDTH - BORDER_SIZE) / 2;
        private const int CHAT_HEIGHT = 10;

        private const int WORLD_X = CHAT_WIDTH + BORDER_SIZE;
        private const int WORLD_Y = STAT_HEIGHT + BORDER_SIZE;
        private const int WORLD_WITH = (SCREEN_WIDTH - BORDER_SIZE * 3) / 2;
        private const int WORLD_HEIGHT = 10;

        private const int INPUT_X = HEADER_X;
        private const int INPUT_Y = STAT_HEIGHT + CHAT_HEIGHT + (BORDER_SIZE * 2);

        public GameScreen()
        {
            _gameStatScreen = new GameStatScreen(STAT_X, STAT_Y, STAT_WIDTH, STAT_HEIGHT);
            _gameChatScreen = new GameChatScreen(CHAT_X, CHAT_Y, CHAT_WIDTH, CHAT_HEIGHT);
            _gameWorldScreen = new GameWorldScreen(WORLD_X, WORLD_Y, WORLD_WITH, WORLD_HEIGHT);
        }

        public override void DrawScreen()
        {
            _gameStatScreen.DrawScreen();
            _gameChatScreen.DrawScreen();
            _gameWorldScreen.DrawScreen();
            DrawInputBox(INPUT_X, INPUT_Y, "Insert an option");
        } 
    }
}