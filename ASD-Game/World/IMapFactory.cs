using System.Collections.Generic;

namespace WorldGeneration
{
    public interface IMapFactory
    {
        Map GenerateMap(int chunkSize, int seed/*, IItemService itemService, List<ItemSpawnDTO> items*/);

        //Map GenerateMap(IItemService itemService, List<ItemSpawnDTO> items, int seed);
        int GenerateSeed();
    }
}