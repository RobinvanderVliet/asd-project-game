namespace UserInterface
{
    public interface IScreenHandler
    {
        public Screen Screen { get; set; }
        public ConsoleHelper ConsoleHelper { get; set; }
        void TransitionTo(Screen screen);
        void DisplayScreen();
        public string GetScreenInput();
        public void SetScreenInput(string input);
    }
}