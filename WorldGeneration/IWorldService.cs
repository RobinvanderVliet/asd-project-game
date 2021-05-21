using System;
using DataTransfer.DTO.Character;

namespace WorldGeneration
{
    public interface IWorldService
    {
        void UpdateCharacterPosition(MapCharacterDTO mapCharacterDto);
        void AddCharacterToWorld(MapCharacterDTO characterPositionDTO, bool isCurrentPlayer);
        void DisplayWorld();
        void DeleteMap();
        void GenerateWorld(int seed);
        MapCharacterDTO getCurrentCharacterPositions();
    }
}