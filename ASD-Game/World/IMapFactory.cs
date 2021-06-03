using ASD_project.World.Services;

namespace ASD_project.World
{
    public interface IMapFactory
    {
        IMap GenerateMap(int chunkSize, int seed, IItemService itemService );
        IMap GenerateMap(IItemService itemService,int seed);
        int GenerateSeed();
    }
}