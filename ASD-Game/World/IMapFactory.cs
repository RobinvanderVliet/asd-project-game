using System.Collections.Generic;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.Items.Services;

namespace ASD_Game.World
{
    public interface IMapFactory
    {
        IMap GenerateMap(int chunkSize, int seed, IItemService itemService, List<ItemSpawnDTO> items);
        IMap GenerateMap(IItemService itemService, List<ItemSpawnDTO> items, int seed);
        int GenerateSeed();
    }
}