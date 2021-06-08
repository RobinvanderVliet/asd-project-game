namespace UserInterface
{
    public interface IScreen
    {
        public abstract void DrawScreen();

        public void SetScreen(ScreenHandler screenHandler);

        public void DrawBox(int x, int y, int innerWidth, int innerHeight);

        public void DrawHeader(string message);

        public void DrawInputBox(int x, int y, string message);
    }
}