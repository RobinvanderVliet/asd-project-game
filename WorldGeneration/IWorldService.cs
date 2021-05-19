using DataTransfer.DTO.Character;

namespace WorldGeneration
{
    public interface IWorldService
    {
        void UpdateCharacterPosition(MapCharacterDTO mapCharacterDto);
        void AddCharacterToWorld(MapCharacterDTO characterPositionDTO);
        void DisplayWorld();
        void DeleteMap();
    }
}