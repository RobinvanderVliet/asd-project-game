using System;
using System.Diagnostics.CodeAnalysis;
using ASD_project.World.Models;
using ASD_project.World.Services;
using DatabaseHandler.Services;

namespace ASD_project.World
{
    public class MapFactory: IMapFactory
    {
        [ExcludeFromCodeCoverage]
        public IMap GenerateMap(IItemService itemService, int seed = 0 )
        {
            return GenerateMap(8, seed, itemService);
            // default chunksize is 8. Can be adjusted in the line above
        }
        
        [ExcludeFromCodeCoverage]
        public IMap GenerateMap(int chunkSize, int seed, IItemService itemservice)
        {
            // If seed is 0 it becomes random
            if (seed == 0)
            {
                seed = GenerateSeed();
            }

            return new Map(new NoiseMapGenerator(seed, itemservice), chunkSize, new DatabaseService<Chunk>(), seed);
        }

        public int GenerateSeed()
        {
            return new Random().Next(1, 9999999);
        }
    }
}