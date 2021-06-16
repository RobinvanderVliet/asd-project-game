using System.Collections.Generic;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.Items.Services;
using ASD_Game.Session.GameConfiguration;
using ASD_Game.World.Models.Characters;

namespace ASD_Game.World
{
    public interface IMapFactory
    {
        IMap GenerateMap(int chunkSize, int seed, IItemService itemService, IEnemySpawner enemySpawner, List<ItemSpawnDTO> items, List<Monster> monsters, IGameConfigurationHandler gameConfigurationHandler);

        IMap GenerateMap(IItemService itemService, IEnemySpawner enemySpawner, List<ItemSpawnDTO> items, List<Monster> monsters, IGameConfigurationHandler gameConfigurationHandler, int seed);

        int GenerateSeed();
    }
}