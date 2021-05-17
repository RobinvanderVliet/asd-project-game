using Player.Services;

namespace Player.DTO
{
    public class MoveDTO
    {
        public PlayerDTO Player{ get; set; }
        public MoveDTO(PlayerDTO player)
        {
            Player = player;
        }
    }
}