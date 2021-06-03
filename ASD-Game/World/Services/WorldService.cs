using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ActionHandling;
using ActionHandling.DTO;
using Items;
using UserInterface;

namespace WorldGeneration
{
    [ExcludeFromCodeCoverage]
    public class WorldService : IWorldService
    {
        private IScreenHandler _screenHandler;
        private ISpawnHandler _spawnHandler;
        private IItemService _itemService;
        private World _world;

        public WorldService(IScreenHandler screenHandler, IItemService itemService)
        {
            _screenHandler = screenHandler;
            _itemService = itemService;
        }

        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition)
        {
            _world.UpdateCharacterPosition(userId, newXPosition, newYPosition);
        }

        public void AddPlayerToWorld(Player player, bool isCurrentPlayer)
        {
            _world.AddPlayerToWorld(player, isCurrentPlayer);
        }

        public void DisplayWorld()
        {
            _world.UpdateMap();
        }
        
        public void DeleteMap()
        {
            _world.DeleteMap();
        }

        public void GenerateWorld(int seed)
        {
            _world = new World(seed, 6, new MapFactory(), _screenHandler, _itemService);
        }

        public Player getCurrentPlayer()
        {
            return _world.CurrentPlayer;
        }

        public List<ItemSpawnDTO> getAllItems()
        {
            return _world._items;
        }

        public void AddItemToWorld(ItemSpawnDTO itemSpawnDto)
        {
            _world.AddItemToWorld(itemSpawnDto);
        }

        public World GetWorld()
        {
            return _world;
        }

        public char[,] GetMapAroundCharacter(Character character)
        {
            return _world.GetMapAroundCharacter(character);
        }
    }
}