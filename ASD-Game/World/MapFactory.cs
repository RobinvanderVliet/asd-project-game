using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.Items.Services;

namespace ASD_Game.World
{
    public class MapFactory: IMapFactory
    {
        public IMap GenerateMap(IItemService itemService, List<ItemSpawnDTO> items, int seed = 0)
        {
            return GenerateMap(8, seed, itemService, items);
            // default chunksize is 8. Can be adjusted in the line above
        }
        
        public IMap GenerateMap(int chunkSize, int seed, IItemService itemService, List<ItemSpawnDTO> items)
        {
            // If seed is 0 it becomes random
            if (seed == 0)
            {
                seed = GenerateSeed();
            }

            return new Map(new NoiseMapGenerator(seed, itemService, items), chunkSize);
        }

        public int GenerateSeed()
        {
            return new Random().Next(1, 9999999);
        }
    }
}