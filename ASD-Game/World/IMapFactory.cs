using System.Collections.Generic;
using ASD_project.ActionHandling.DTO;
using ASD_project.Items.Services;

namespace ASD_project.World
{
    public interface IMapFactory
    {
        Map GenerateMap(int chunkSize, int seed, IItemService itemService, List<ItemSpawnDTO> items);
        Map GenerateMap(IItemService itemService, List<ItemSpawnDTO> items, int seed);
        int GenerateSeed();
    }
}