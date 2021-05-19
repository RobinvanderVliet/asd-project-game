namespace Player.DTO
{
    public class PlayerDTO
    {
        public string PlayerName;
        public string PlayerGuid;
        public int X;
        public int Y;

        public PlayerDTO(string playerName, int x, int y, string playerGuid)
        {
            PlayerName = playerName;
            X = x;
            Y = y;
            PlayerGuid = playerGuid;
        }
    }
}