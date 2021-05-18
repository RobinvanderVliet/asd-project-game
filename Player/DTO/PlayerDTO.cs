namespace Player.DTO
{
    public class PlayerDTO
    {
        public string PlayerName;
        public int X;
        public int Y;
        public int Team;

        public PlayerDTO(string playerName, int x, int y, int team = 0)
        {
            PlayerName = playerName;
            X = x;
            Y = y;
            Team = team;
        }
    }
}