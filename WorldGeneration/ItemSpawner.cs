using System;
using Items;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.LootableTiles;

namespace WorldGeneration
{
    public class ItemSpawner : ISpawner
    {
        private IFastNoise _noise;

        public ItemSpawner(IFastNoise noise)
        {
            _noise = noise;
        }
        
        public void Spawn(Chunk chunk)
        {
            // Determine if an item should be spawned in the chunk. For now this will always be true.

            Boolean shouldItemSpawn = true;
            if (shouldItemSpawn)
            {
                var chestTile = new ChestTile();
                chestTile.ItemsOnTile.Add(ItemFactory.GetKnife());

                var noiseresult = _noise.GetNoise(chunk.X, chunk.Y);
                var randomTile = (int) ((chunk.RowSize * chunk.RowSize - 1) * noiseresult );
                if (randomTile < 0)
                {
                    randomTile = randomTile * -1;
                }
                chestTile.XPosition = chunk.Map[randomTile].XPosition;
                chestTile.YPosition = chunk.Map[randomTile].YPosition;
                chunk.Map[randomTile] = chestTile;
            }
        }
    }
}