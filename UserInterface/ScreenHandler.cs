using System;
using System.Collections.Generic;

namespace UserInterface
{
    public class ScreenHandler : IScreenHandler
    {
        private Screen _screen = null;
        // public Screen Screen { get => _screen; }
        public Screen Screen { get => _screen; set => _screen = value; }
        
        public void TransitionTo(Screen screen)
        {
            Console.Clear();
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
            if(_screen is GameScreen)
            {
                var gameScreen = Screen as GameScreen;
                gameScreen.ShowMessages(messages);
            }
        }
    }
}