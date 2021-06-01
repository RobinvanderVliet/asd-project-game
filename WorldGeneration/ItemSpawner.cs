using Items;
using WorldGeneration.Models;
using WorldGeneration.Models.LootableTiles;

namespace WorldGeneration
{
    public class ItemSpawner : ISpawner
    {
        private FastNoiseLite _noise;
        
        public void Spawn(Chunk chunk)
        {
            // Determine if an item should be spawned in the chunk. For testing this will be true.
            var chestTile = new ChestTile();
            chestTile.ItemsOnTile.Add(ItemFactory.GetKnife());

            var randomTile = (int) ((chunk.RowSize * chunk.RowSize - 1) * _noise.GetNoise(chunk.X, chunk.Y));
            chestTile.XPosition = chunk.Map[randomTile].XPosition;
            chestTile.YPosition = chunk.Map[randomTile].YPosition;
            
            throw new System.NotImplementedException();
        }
    }
}