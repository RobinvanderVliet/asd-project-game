namespace UserInterface
{
    public interface IGameStatScreen : IScreen
    {
        public void SetStatValues(string name, int score, int health, int stamina, int armor, int radiation, string helm, string body, string weapon, string slotOne, string slotTwo, string slotThree);
    }
}