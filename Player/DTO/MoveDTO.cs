using DataTransfer.DTO.Player;
using Player.Services;

namespace Player.DTO
{
    public class MoveDTO
    {
        public PlayerPositionDTO PlayerPosition{ get; set; }
        public MoveDTO(PlayerPositionDTO playerPosition)
        {
            PlayerPosition = playerPosition;
        }
    }
}