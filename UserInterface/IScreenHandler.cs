namespace UserInterface
{
    public interface IScreenHandler
    {
        public Screen Screen { get; set; }
        void TransitionTo(Screen screen);
        void DisplayScreen();
        public void SetStatValues(string name, int score, int health, int stamina, int armor, int radiation, string helm, string body, string weapon, string slotOne, string slotTwo, string slotThree);
    }
}