using System.Collections.Generic;

namespace UserInterface
{
    public interface IScreenHandler
    {
        public Screen Screen { get; set; }
        void TransitionTo(Screen screen);
        void DisplayScreen();
        public ConsoleHelper ConsoleHelper { get; set; }
        public string GetScreenInput();
        public void ShowMessages(Queue<string> messages);
        public void RedrawGameInputBox();
        void UpdateWorld(char[,] map);
    }
}
       
        