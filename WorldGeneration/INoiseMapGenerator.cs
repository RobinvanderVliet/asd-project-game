using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
{
    public interface INoiseMapGenerator
    {
        public Chunk GenerateChunk(int chunkX, int chunkY, int chunkRowSize, int seed);

    }
}