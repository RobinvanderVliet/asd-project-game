using System;

namespace UserInterface
{
    public class ScreenHandler
    {
        private Screen _screen = null;
        
        public void TransitionTo(Screen screen)
        {
            _screen = screen;
            _screen.SetScreen(this);
        }
        public void DisplayScreen()
        {
            // if (clearScreen)
            // {
            //     Console.Clear();
            // }
            
            _screen.DrawScreen();
        }
    }
}