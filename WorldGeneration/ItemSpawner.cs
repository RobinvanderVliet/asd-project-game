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
            var noiseresult = _noise.GetNoise(chunk.X, chunk.Y);
            var numberOfItemSpawns = 0;

            switch (noiseresult)
            {
                case <-0.8f:
                    // do not change numberOfItemSpawns.
                    break;
                case <-0.2f:
                    numberOfItemSpawns = 1;
                    break;
                case <0.4f:
                    numberOfItemSpawns = 2;
                    break;
                case <= 1:
                    numberOfItemSpawns = 3;
                    break;
                default:
                    numberOfItemSpawns = 0;
                    break;
            }

            for (int i = 0; i < numberOfItemSpawns; i++)
            {
                var randomTile = (int) ((chunk.RowSize * chunk.RowSize - 1) * noiseresult );
                if (randomTile < 0)
                {
                    randomTile *= -1;
                }
                chunk.Map[randomTile].ItemsOnTile.Add(ItemFactory.GetKnife());
            }
        }
    }
}