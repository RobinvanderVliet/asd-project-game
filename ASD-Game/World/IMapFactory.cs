using System.Collections.Generic;
using ActionHandling.DTO;
using ASD_project.World.Services;

namespace ASD_project.World
{
    public interface IMapFactory
    {
        IMap GenerateMap(int chunkSize, int seed, IItemService itemService, List<ItemSpawnDTO> items);
        IMap GenerateMap(IItemService itemService, List<ItemSpawnDTO> items, int seed);
        int GenerateSeed();
    }
}