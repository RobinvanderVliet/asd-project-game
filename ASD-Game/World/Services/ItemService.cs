using ActionHandling;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
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
    }
}