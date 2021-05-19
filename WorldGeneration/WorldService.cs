using System;
using DataTransfer.DTO.Character;

namespace WorldGeneration
{
    public class WorldService : IWorldService
    {
        private World _world;

        public WorldService()
        {
            // _world = new World();
        }

        public void UpdateCharacterPosition(MapCharacterDTO mapCharacterDto)
        {
            _world.UpdateCharacterPosition(mapCharacterDto);
        }

        public void AddCharacterToWorld(MapCharacterDTO mapCharacterDto, Boolean isCurrentPlayer)
        {
            _world.AddCharacterToWorld(mapCharacterDto, isCurrentPlayer);
        }

        public void DisplayWorld()
        {
            _world.DisplayWorld();
        }
        
        public void DeleteMap()
        {
            _world.deleteMap();
        }

        public void GenerateWorld(int seed)
        {
            _world = new World(seed, 6);
        }
    }
}