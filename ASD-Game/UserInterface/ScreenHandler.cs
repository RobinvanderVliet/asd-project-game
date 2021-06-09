using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ASD_Game.UserInterface
{
    public class ScreenHandler : IScreenHandler
    {
        private Screen _screen = null;
        public Screen Screen { get => _screen; set => _screen = value; }
        private ConsoleHelper _consoleHelper;
        public ConsoleHelper ConsoleHelper { get => _consoleHelper; set => _consoleHelper = value; }
        private BlockingCollection<Action> _actionsInQueue;
        private Thread _displayThread { get; set; }
        
        private bool _displaying;
        
        public ScreenHandler()
        {
            _consoleHelper = new ConsoleHelper();
            _actionsInQueue = new();

            _displayThread = new Thread(RunDisplay);
            _displayThread.Start();
        }

        private void RunDisplay()
        {
            while(_actionsInQueue.TryTake(out Action a, -1))
            {
                a.Invoke();
            }
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
            _actionsInQueue.Add(_screen.DrawScreen);
        }

        public void ShowMessages(Queue<string> messages)
        {
            
            if (_screen is GameScreen)
            {
                var gameScreen = Screen as GameScreen;
                _actionsInQueue.Add(() => gameScreen.ShowMessages(messages));
            }
        }

        public virtual string GetScreenInput()
        {
            return _consoleHelper.ReadLine();
        }

        public void RedrawGameInputBox()
        {
            if (_screen is GameScreen)
            {
                var gameScreen = Screen as GameScreen;
                _actionsInQueue.Add(gameScreen.RedrawInputBox);
                _displayThread = new Thread(gameScreen.RedrawInputBox);
            }
        }

        public void UpdateWorld(char[,] map)
        {
            if (_screen is GameScreen)
            {
                var gameScreen = Screen as GameScreen;
                _actionsInQueue.Add(() => gameScreen.UpdateWorld(map));
            }
        }

        public void SetStatValues(string name, int score, int health, int stamina, int armor, int radiation, string helm, string body, string weapon, string slotOne, string slotTwo, string slotThree)
        {
            if (_screen is GameScreen)
            {
                GameScreen gameScreen = _screen as GameScreen;
                _actionsInQueue.Add(() => gameScreen.SetStatValues(name, score, health, stamina, armor, radiation, helm, body, weapon, slotOne, slotTwo, slotThree));
            }
        }

        public virtual void SetScreenInput(string input)
        {
            _consoleHelper.WriteLine(input);
        }

        public void UpdateInputMessage(string message)
        {
            if (_screen is LoadScreen screen)
            {
                screen.UpdateInputMessage(message);
            }
        }

        public string GetSessionByPosition(int sessionNumber)
        {
            if (_screen is LoadScreen screen)
            {
                return screen.GetSessionByPosition(sessionNumber);
            }
            
            return String.Empty;
        }
    }
}