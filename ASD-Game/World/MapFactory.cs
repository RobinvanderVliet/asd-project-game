using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.Items.Services;
using ASD_Game.World.Models.Characters;
using Moq;

namespace ASD_Game.World
{
    public class MapFactory: IMapFactory
    {
        public IMap GenerateMap(IItemService itemService, IEnemySpawner enemySpawner, List<ItemSpawnDTO> items, List<Monster> monsters, int seed = 0)
        {
            return GenerateMap(8, seed, itemService, enemySpawner, items, monsters);
            // default chunksize is 8. Can be adjusted in the line above
        }
        
        public IMap GenerateMap(int chunkSize, int seed, IItemService itemService, IEnemySpawner enemySpawner, List<ItemSpawnDTO> items, List<Monster> monsters)
        {
            // If seed is 0 it becomes random
            if (seed == 0)
            {
                seed = GenerateSeed();
            }

            return new Map(new NoiseMapGenerator(seed, itemService, enemySpawner, items, monsters), chunkSize);
        }

        public int GenerateSeed()
        {
            return new Random().Next(1, 9999999);
        }
    }
}