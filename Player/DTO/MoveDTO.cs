using DataTransfer.DTO.Character;

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