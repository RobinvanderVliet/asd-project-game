using System.Collections.Generic;
using DataTransfer.DTO.Character;

namespace WorldGeneration
{
    public class WorldService
    {
        private World _world;

        public WorldService(World world)
        {
            _world = world;
        }

        public void UpdateCharacterPosition(MapCharacterDTO mapCharacterDto)
        {
            _world.UpdateCharacterPosition(mapCharacterDto);
        }

        public void displayWorld()
        {
            _world.DisplayWorld();
        }
        
        public List<MapCharacterDTO> getPlayerList()
        {
            return _world.Characters;
        }
    }
}