using DataTransfer.DTO.Character;

namespace WorldGeneration
{
    public class WorldService : IWorldService
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

        public void AddCharacterToWorld(MapCharacterDTO characterPositionDTO)
        {
            _world.AddCharacterToWorld(characterPositionDTO);
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