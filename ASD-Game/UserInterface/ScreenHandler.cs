using System.Collections.Generic;

namespace ASD_Game.UserInterface
{
    public class ScreenHandler : IScreenHandler
    {
        private Screen _screen = null;
        public Screen Screen { get => _screen; set => _screen = value; }
        private ConsoleHelper _consoleHelper;
        public ConsoleHelper ConsoleHelper { get => _consoleHelper; set => _consoleHelper = value; }
        private bool _displaying;
        
        public ScreenHandler()
        {
            _consoleHelper = new ConsoleHelper();
            _displaying = false;
        }
        public void TransitionTo(Screen screen)
        {
            _consoleHelper.ClearConsole();
            _screen = screen;
            _screen.SetScreen(this);
            DisplayScreen();
        }
        public void DisplayScreen()
        {
            _screen.DrawScreen();
        }

        public void ShowMessages(Queue<string> messages)
        {
            if (!_displaying)
            {
                _displaying = true;
                if(_screen is GameScreen)
                {
                    var gameScreen = Screen as GameScreen;
                    gameScreen.ShowMessages(messages);
                }
                _displaying = false;
            }
        }

        public string GetScreenInput()
        {
            return _consoleHelper.ReadLine();
        }

        public void RedrawGameInputBox()
        {
            if (_screen is GameScreen)
            {
                var gameScreen = Screen as GameScreen;
                gameScreen.RedrawInputBox();
            }
        }

        public void UpdateWorld(char[,] map)
        {
            if (!_displaying)
            {
                _displaying = true;
                if (_screen is GameScreen)
                {
                    var gameScreen = Screen as GameScreen;
                    gameScreen.UpdateWorld(map);
                }
                _displaying = false;
            }
        }

        public void SetStatValues(string name, int score, int health, int stamina, int armor, int radiation, string helm, string body, string weapon, string slotOne, string slotTwo, string slotThree)
        {
            if (!_displaying)
            {
                _displaying = true;
                if (_screen is GameScreen)
                {
                    GameScreen gameScreen = _screen as GameScreen;
                    gameScreen.SetStatValues(name, score, health, stamina, armor, radiation, helm, body, weapon, slotOne, slotTwo, slotThree);
                }
                _displaying = false;
            }
        }
    }
}