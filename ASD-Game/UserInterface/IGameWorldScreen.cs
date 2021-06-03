namespace UserInterface
{
    public interface IGameWorldScreen: IScreen
    {
        public void UpdateWorld(char[,] newMap);
    }
}
