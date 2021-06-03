using WorldGeneration.Models;

namespace WorldGeneration
{
    public interface ISpawner
    {
        void Spawn(Chunk chunk);
    }
}