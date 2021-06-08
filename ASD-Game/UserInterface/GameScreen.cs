using System.Collections.Generic;

namespace ASD_Game.UserInterface
{
    public class GameScreen : Screen
    {
        private IGameStatScreen _gameStatScreen;
        private IGameChatScreen _gameChatScreen;
        private IGameWorldScreen _gameWorldScreen;

        private const int STAT_X = HEADER_X;
        private const int STAT_Y = HEADER_Y;
        private const int STAT_WIDTH = HEADER_WIDTH;
        private const int STAT_HEIGHT = 5;

        private const int CHAT_X = HEADER_X;
        private const int CHAT_Y = STAT_HEIGHT + BORDER_SIZE;
        private const int CHAT_WIDTH = (SCREEN_WIDTH - BORDER_SIZE) - (WORLD_WITDH + BORDER_SIZE);
        private const int CHAT_HEIGHT = WORLD_HEIGHT;

        private const int WORLD_X = CHAT_WIDTH + BORDER_SIZE;
        private const int WORLD_Y = STAT_HEIGHT + BORDER_SIZE;

        private const int VIEW_DISTANCE = 6;
        private const int WORLD_WIDTH_BLANK_SPACE = 2;
        private const int CENTER_LINE = 1;
        private const int WORLD_HEIGHT = VIEW_DISTANCE * 2 + CENTER_LINE;
        private const int WORLD_WITDH = (((VIEW_DISTANCE * 2) + CENTER_LINE) * (WORLD_WIDTH_BLANK_SPACE + 1)) - WORLD_WIDTH_BLANK_SPACE;
        private const int INPUT_X = HEADER_X;
        private const int INPUT_Y = STAT_HEIGHT + WORLD_HEIGHT + (BORDER_SIZE * 2);

        public GameScreen()
        {
            _gameStatScreen = new GameStatScreen(STAT_X, STAT_Y, STAT_WIDTH, STAT_HEIGHT);
            _gameChatScreen = new GameChatScreen(CHAT_X, CHAT_Y, CHAT_WIDTH, CHAT_HEIGHT);
            _gameWorldScreen = new GameWorldScreen(WORLD_X, WORLD_Y, WORLD_WITDH, WORLD_HEIGHT);
        }

        public void setScreens(IGameStatScreen gameStatScreen, IGameChatScreen gameChatScreen, IGameWorldScreen gameWorldScreen)
        {
            _gameStatScreen = gameStatScreen;
            _gameChatScreen = gameChatScreen;
            _gameWorldScreen = gameWorldScreen;
        }

        public override void DrawScreen()
        {
            _gameStatScreen.SetScreen(_screenHandler);
            _gameChatScreen.SetScreen(_screenHandler);
            _gameWorldScreen.SetScreen(_screenHandler);
            _screenHandler.ConsoleHelper.SetColorToGreen();
            _gameStatScreen.DrawScreen();
            _gameChatScreen.DrawScreen();
            _gameWorldScreen.DrawScreen();
            DrawInputBox(INPUT_X, INPUT_Y, "Insert an option");
        }

        public void RedrawInputBox()
        {
            for(int i = INPUT_Y; i < _screenHandler.ConsoleHelper.GetConsoleHeight(); i++)
            {
                _screenHandler.ConsoleHelper.SetCursor(INPUT_X, i);
                _screenHandler.ConsoleHelper.Write(new string(' ', _screenHandler.ConsoleHelper.GetConsoleWidth()));
            }           
            DrawInputBox(INPUT_X, INPUT_Y, "Insert an option");
        }

        public void ShowMessages(Queue<string> messages)
        {
            _gameChatScreen.ShowMessages(messages);
        }

        public void UpdateWorld(char[,] newMap)
        {
            _gameWorldScreen.UpdateWorld(newMap);
        }

        public void SetStatValues(string name, int score, int health, int stamina, int armor, int radiation, string helm, string body, string weapon, string slotOne, string slotTwo, string slotThree)
        {
            _gameStatScreen.SetStatValues(name, score, health, stamina, armor, radiation, helm, body, weapon, slotOne, slotTwo, slotThree);
        }
    }
}