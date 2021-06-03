using ASD_project.World.Models;

namespace ASD_project.World
{
    public interface INoiseMapGenerator
    {
        public Chunk GenerateChunk(int chunkX, int chunkY, int chunkRowSize);
    }
}