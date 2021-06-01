using WorldGeneration.Models;

namespace WorldGeneration
{
    public interface INoiseMapGenerator
    {
        public Chunk GenerateChunk(int chunkX, int chunkY, int chunkRowSize, int seed);
    }
}