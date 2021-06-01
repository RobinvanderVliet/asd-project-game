using System;

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

        public void SetStatValues(string name, int score, int health, int stamina, int armor, int radiation, string helm, string body, string weapon, string slotOne, string slotTwo, string slotThree)
        {
            if (_screen is GameScreen)
            {
                GameScreen gameScreen = _screen as GameScreen;
                gameScreen.SetStatValues(name, score, health, stamina, armor, radiation, helm, body, weapon, slotOne, slotTwo, slotThree);
            }
        }
    }
}