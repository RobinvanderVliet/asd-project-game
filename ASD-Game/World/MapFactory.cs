using System;

namespace WorldGeneration
{
    public abstract class MapFactory
    {
        public static Map GenerateMap(string dbLocation = "Filename=C:\\Temp\\ChunkDatabase.db;connection=shared;", String collectionName = "ChunkMap", int chunkSize = 8, int seed = -1123581321)
        {
            // Default chunk size is 8. Can be adjusted in the line above.
            // Seed can be null, if it is it becomes random. But because of how C# works you can't set a default null, so this workaround exists.
            if (seed == -1123581321)
            {
                seed = new Random().Next(1, 999999);
            }

            return new Map(new NoiseMapGenerator(), new DatabaseFunctions.Database(dbLocation, collectionName), chunkSize, seed);
        }

        public static int GenerateSeed()
        {
            var randomSeed = new Random().Next(1, 999999);
            return randomSeed;
        }
    }
}