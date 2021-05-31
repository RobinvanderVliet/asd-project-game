using System.Reflection;

namespace WorldGeneration
{
    public interface IMapFactory
    {
        IMap GenerateMap(int chunkSize, int seed);
        IMap GenerateMap(int seed);
        int GenerateSeed();
    }
}