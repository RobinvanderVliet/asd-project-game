namespace ASD_Game.UserInterface
{
    public interface IGameWorldScreen : IScreen
    {
        public void UpdateWorld(char[,] newMap);
    }
}
