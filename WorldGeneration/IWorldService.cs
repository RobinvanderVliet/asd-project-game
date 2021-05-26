using System;
using DataTransfer.DTO.Character;

namespace WorldGeneration
{
    public interface IWorldService
    {
        public void UpdateCharacterPosition(MapCharacterDTO mapCharacterDTO);
        public void AddCharacterToWorld(MapCharacterDTO characterPositionDTO, bool isCurrentPlayer);
        public void DisplayWorld();
        public void DeleteMap();
        public void GenerateWorld(int seed);
        public MapCharacterDTO getCurrentCharacterPositions();
    }
}