using System.Collections.Generic;

namespace ASD_Game.UserInterface
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
        public void SetStatValues(string name, int score, int health, int stamina, int armor, int radiation, string helm, string body, string weapon, string slotOne, string slotTwo, string slotThree);
    }
}