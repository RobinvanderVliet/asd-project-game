using ASD_Game.World.Models;

namespace ASD_Game.World
{
    public interface INoiseMapGenerator
    {
        public Chunk GenerateChunk(int chunkX, int chunkY, int chunkRowSize);
    }
}