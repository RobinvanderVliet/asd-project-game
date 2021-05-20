using System;
using DataTransfer.DTO.Character;

namespace WorldGeneration
{
    public interface IWorldService
    {
        void UpdateCharacterPosition(MapCharacterDTO mapCharacterDto);
        void AddCharacterToWorld(MapCharacterDTO characterPositionDTO, Boolean isCurrentPlayer);
        void DisplayWorld();
        void DeleteMap();
        void GenerateWorld(int seed);
        MapCharacterDTO getCurrentCharacterPositions();
    }
}