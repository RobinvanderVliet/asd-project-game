using System.Collections.Generic;

namespace UserInterface
{
    public interface IScreenHandler
    {
        public Screen Screen { get; set; }
        void TransitionTo(Screen screen);
        void DisplayScreen();

        public void ShowMessages(Queue<string> messages);
    }
}