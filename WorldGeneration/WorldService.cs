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

        public void DisplayWorld()
        {
            _world.DisplayWorld();
        }
        
        public void DeleteMap()
        {
            _world.deleteMap();
        }
    }
}