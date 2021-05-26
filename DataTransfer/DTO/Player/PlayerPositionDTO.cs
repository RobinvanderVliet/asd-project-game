namespace DataTransfer.DTO.Player
{
    public class PlayerPositionDTO
    {
        public int X;
        public int Y;
        public string PlayerName;
        public int Team;

        public PlayerPositionDTO(int x, int y, string playerName, int team = 0)
        {
            X = x;
            Y = y;
            PlayerName = playerName;
            Team = team;
        }
    }
}