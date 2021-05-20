using DataTransfer.DTO.Character;
using DataTransfer.DTO.Player;
using Player.Services;

namespace Player.DTO
{
    public class MoveDTO
    {
        public MapCharacterDTO PlayerPosition{ get; set; }
        public MoveDTO(MapCharacterDTO playerPosition)
        {
            PlayerPosition = playerPosition;
        }

    }
}