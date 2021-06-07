using DatabaseHandler.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using WorldGeneration.Models;

namespace WorldGeneration
{
    public class MapFactory : IMapFactory
    {
        public static Map GenerateMap(string dbLocation, string collectionName = "ChunkMap", int chunkSize = 8, int seed = -1123581321)
        {
            // Default chunk size is 8. Can be adjusted in the line above.
            // Seed can be null, if it is it becomes random. But because of how C# works you can't set a default null, so this workaround exists.
            if (seed == -1123581321)
            {
                seed = new Random().Next(1, 999999);
            }
            return new Map(new NoiseMapGenerator(seed), chunkSize, new DatabaseService<Chunk>(), seed);
        }

        public Map GenerateMap(int chunkSize, int seed)
        {
            throw new NotImplementedException();
        }

        public int GenerateSeed()
        {
            return new Random().Next(1, 9999999);
        }
    }
}