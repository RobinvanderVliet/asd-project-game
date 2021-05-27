using DataTransfer.DTO.Character;
using System.Diagnostics.CodeAnalysis;

namespace Player.DTO
{
    [ExcludeFromCodeCoverage]
    public class MoveDTO
    {
        public MapCharacterDTO PlayerPosition { get; set; }
        public MoveDTO(MapCharacterDTO playerPosition)
        {
            PlayerPosition = playerPosition;
        }

    }
}