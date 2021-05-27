namespace Session
{
    public interface IGamesSessionService
    {
        public void RequestSavedGames();
        public void StartGame();
        public void LoadGame(string value);
    }
}