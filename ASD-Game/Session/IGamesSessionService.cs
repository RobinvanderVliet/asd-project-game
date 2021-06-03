namespace Session
{
    public interface IGamesSessionService
    {
        public void RequestSavedGames();
        public void LoadGame(string value);
    }
}