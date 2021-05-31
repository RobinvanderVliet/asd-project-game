using DataTransfer.Model.World;

namespace WorldGeneration
{
    public interface INoiseMapGenerator
    {
        public Chunk GenerateChunk(int chunkX, int chunkY, int chunkRowSize, int seed);
    }
}