using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_project.ActionHandling.DTO;
using ASD_project.DatabaseHandler.Services;
using ASD_project.Items.Services;
using ASD_project.World.Models;

namespace ASD_project.World
{
    public class MapFactory: IMapFactory
    {
        [ExcludeFromCodeCoverage]
        public Map GenerateMap(IItemService itemService, List<ItemSpawnDTO> items, int seed = 0)
        {
            return GenerateMap(8, seed, itemService, items);
            // default chunksize is 8. Can be adjusted in the line above
        }
        
        [ExcludeFromCodeCoverage]
        public Map GenerateMap(int chunkSize, int seed, IItemService itemService, List<ItemSpawnDTO> items)
        {
            // If seed is 0 it becomes random
            if (seed == 0)
            {
                seed = GenerateSeed();
            }

            return new Map(new NoiseMapGenerator(seed, itemService, items), chunkSize, new DatabaseService<Chunk>());
        }

        public int GenerateSeed()
        {
            return new Random().Next(1, 9999999);
        }
    }
}