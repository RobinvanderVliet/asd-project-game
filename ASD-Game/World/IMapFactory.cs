using System.Reflection;
using ActionHandling;

namespace WorldGeneration
{
    public interface IMapFactory
    {
        IMap GenerateMap(int chunkSize, int seed, IItemService itemService );
        IMap GenerateMap(IItemService itemService,int seed);
        int GenerateSeed();
    }
}