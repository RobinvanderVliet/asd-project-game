using System.Collections.Generic;
using System.Threading;

namespace UserInterface
{
    public class ScreenHandler : IScreenHandler
    {
        private Screen _screen = null;
        public Screen Screen { get => _screen; set => _screen = value; }
        private ConsoleHelper _consoleHelper;
        public ConsoleHelper ConsoleHelper { get => _consoleHelper; set => _consoleHelper = value; }
        private bool _displaying;
        public static Thread DisplayThread { get; set; }
        
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
            if(DisplayThread != null)
            {
                DisplayThread.Join();
            }
            DisplayThread = new Thread(_screen.DrawScreen);
            DisplayThread.Start();
        }

        public void ShowMessages(Queue<string> messages)
        {
            if (_screen is GameScreen)
            {
                var gameScreen = Screen as GameScreen;
                DisplayThread.Join();
                DisplayThread = new Thread(empty => gameScreen.ShowMessages(messages));
                DisplayThread.Start();
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
                DisplayThread.Join();
                DisplayThread = new Thread(gameScreen.RedrawInputBox);
                DisplayThread.Start();
            }
        }

        public void UpdateWorld(char[,] map)
        {
            if (_screen is GameScreen)
            {
                var gameScreen = Screen as GameScreen;
                DisplayThread.Join();
                DisplayThread = new Thread(empty => gameScreen.UpdateWorld(map));
                DisplayThread.Start();
            }
        }

        public void SetStatValues(string name, int score, int health, int stamina, int armor, int radiation, string helm, string body, string weapon, string slotOne, string slotTwo, string slotThree)
        {
            if (_screen is GameScreen)
            {
                GameScreen gameScreen = _screen as GameScreen;
                DisplayThread.Join();
                DisplayThread = new Thread(empty => gameScreen.SetStatValues(name, score, health, stamina, armor, radiation, helm, body, weapon, slotOne, slotTwo, slotThree));
                DisplayThread.Start();
            }
        }
    }
}