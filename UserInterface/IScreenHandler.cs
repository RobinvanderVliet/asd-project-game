namespace UserInterface
{
    public interface IScreenHandler
    {
        void TransitionTo(Screen screen);
        void DisplayScreen();
        void AcceptInput();
    }
}