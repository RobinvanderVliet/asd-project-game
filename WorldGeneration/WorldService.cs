using System;
using DataTransfer.DTO.Character;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
{
    public class WorldService : IWorldService
    {
        private World _world;

        public void UpdateCharacterPosition(MapCharacterDTO mapCharacterDTO)
        {
            _world.UpdateCharacterPosition(mapCharacterDTO);
        }

        public void AddCharacterToWorld(MapCharacterDTO mapCharacterDTO, bool isCurrentPlayer)
        {
            _world.AddCharacterToWorld(mapCharacterDTO, isCurrentPlayer);
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

        public MapCharacterDTO getCurrentCharacterPositions()
        {
            return _world.CurrentPlayer;
        }

        public ITile GetTile(int x, int y)
        {
            return _world._map.GetLoadedTileByXAndY(x, y);
        }
    }
}