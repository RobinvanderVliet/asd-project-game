using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.Items.Services;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.HazardousTiles;
using ASD_Game.World.Models.Interfaces;
using ASD_Game.World.Models.TerrainTiles;

namespace ASD_Game.World
{
    public class NoiseMapGenerator : INoiseMapGenerator
    {
        private IFastNoise _worldNoise;
        private IFastNoise _itemNoise;
        private IItemService _itemService;
        private List<ItemSpawnDTO> _items;
        private List<Monster> _monsters;
        private int _monsterSpawnChance;
        private IEnemySpawner _enemySpawner;

        public NoiseMapGenerator(int seed, IItemService itemService, IEnemySpawner enemySpawner, List<ItemSpawnDTO> items, List<Monster> monsters, int monsterSpawnChance = 5)
        {
            _worldNoise = new FastNoiseLite();
            _worldNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            _worldNoise.SetFrequency(0.015f);
            _worldNoise.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);
            _worldNoise.SetSeed(seed);
            _itemNoise = new FastNoiseLite();
            _itemNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            _itemNoise.SetFrequency(10f);
            _itemNoise.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);
            _itemNoise.SetSeed(seed);
            _itemService = itemService;
            _items = items;
            _monsters = monsters;
            _monsterSpawnChance = monsterSpawnChance;
            _enemySpawner = enemySpawner;
        }

        public Chunk GenerateChunk(int chunkX, int chunkY, int chunkRowSize)
        {
            var map = new ITile[chunkRowSize * chunkRowSize];
            for (var y = 0; y < chunkRowSize; y++)
            {
                for (var x = 0; x < chunkRowSize; x++)
                {
                    map[y * chunkRowSize + x] = CreateTileWithItemFromNoise(
                        _worldNoise.GetNoise(x + chunkX * chunkRowSize, y + chunkY * chunkRowSize)
                        , _itemNoise.GetNoise(x + chunkX * chunkRowSize, y + chunkY * chunkRowSize)
                        , x + chunkRowSize * chunkX
                        , chunkRowSize * chunkY - chunkRowSize + y);
                }
            }
            return new Chunk(chunkX, chunkY, map, chunkRowSize);
        }

        private ITile CreateTileWithItemFromNoise(float worldNoise, float itemNoise, int x, int y)
        {
            var tile = GetTileFromNoise(worldNoise, x, y);

            if (!tile.IsAccessible)
            {
                return tile;
            }

            SpawnMonsterOnTile(x, y, itemNoise);

            var item = _itemService.GenerateItemFromNoise(itemNoise, x, y);
            var itemSpawnDTO = new ItemSpawnDTO { Item = item, XPosition = x, YPosition = y };

            if (item == null)
            {
                return tile;
            }
            if (_items.Exists(itemInList => itemInList.Item.ItemId == item.ItemId))
            {
                return tile;
            }

            _items.Add(itemSpawnDTO);
            tile.ItemsOnTile.Add(item);

            return tile;
        }

        public ITile GetTileFromNoise(float noise, int x, int y)
        {
            y++;
            return (noise * 10) switch
            {
                (< -8) => new WaterTile(x, y),
                (< -4) => new DirtTile(x, y),
                (< 2) => new GrassTile(x, y),
                (< 3) => new SpikeTile(x, y),
                (< 8) => new StreetTile(x, y),
                _ => new GasTile(x, y)
            };
        }

        private void SpawnMonsterOnTile(int x, int y, float noise)
        {
            if (noise * 100 < -100 + _monsterSpawnChance)
            {
                var id = "monst" + x + "!" + y;
                if (!_monsters.Exists(monster => monster.Id == id))
                {
                    _monsters.Add(_enemySpawner.spawnMonster(x, y, id, 5));
                }
            }
        }

        [ExcludeFromCodeCoverage]
        public void SetNoise(IFastNoise noise)
        {
            _itemNoise = noise;
            _worldNoise = noise;
        }
    }
}