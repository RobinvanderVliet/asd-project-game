using System.Diagnostics.CodeAnalysis;
using DataTransfer.DTO.Character;

namespace Player.DTO
{
    [ExcludeFromCodeCoverage]
    public class MoveDTO
    {
        public MapCharacterDTO PlayerPosition{ get; set; }
        public MoveDTO(MapCharacterDTO playerPosition)
        {
            PlayerPosition = playerPosition;
        }

    }
}