using System;
using System.Diagnostics.CodeAnalysis;
using DatabaseHandler.Services;
using DataTransfer.Model.World;
using Display;

namespace WorldGeneration
{
    public class MapFactory
    {
        [ExcludeFromCodeCoverage]
        public Map GenerateMap(int chunkSize = 8, int seed = 0)
        {
            // default chunksize is 8. Can be adjusted in the line above
            
            // If seed is 0 it becomes random
            if (seed == 0)
            {
                seed = GenerateSeed();
            }

            return new Map(new NoiseMapGenerator(), chunkSize, seed, new DatabaseService<Chunk>(),
                new ConsolePrinter());
        }

        public static int GenerateSeed()
        {
            return new Random().Next(1, 9999999);
        }
    }
}