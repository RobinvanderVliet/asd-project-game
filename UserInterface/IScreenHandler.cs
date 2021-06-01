namespace UserInterface
{
    public interface IScreenHandler
    {
        public Screen Screen { get; set; }
        void TransitionTo(Screen screen);
        void DisplayScreen();
        void UpdateWorld(char[,] map);
    }
}