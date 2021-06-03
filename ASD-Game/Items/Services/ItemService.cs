using ActionHandling;
using ASD_project.World;
using ASD_project.World.Models.Interfaces;
using ASD_project.World.Services;

namespace ASD_project.Items.Services
{
    public class ItemService : IItemService
    {
        private ISpawnHandler _spawnHandler;
        public ItemService(ISpawnHandler spawnHandler)
        {
            _spawnHandler = spawnHandler;
        }
        public ITile PutItemOnTile(ITile tile, float noiseResult)
        {
            var item = RandomItemGenerator.GetRandomItem(noiseResult);
            if (item != null)
            {
                _spawnHandler.SendSpawn(tile.XPosition, tile.YPosition, item);
                tile.ItemsOnTile.Add(item);
            }

            return tile;
        }

        public ISpawnHandler GetSpawnHandler()
        {
            return _spawnHandler;
        }
    }
}