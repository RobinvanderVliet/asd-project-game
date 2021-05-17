namespace Player.DTO
{
    public class PlayerDTO
    {
        public string PlayerName;
        public int X;
        public int Y;

        public PlayerDTO(string playerName, int x, int y)
        {
            PlayerName = playerName;
            X = x;
            Y = y;
        }
    }
}