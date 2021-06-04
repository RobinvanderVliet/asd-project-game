using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ActionHandling.DTO;
using ASD_project.World.Models;
using ASD_project.World.Models.HazardousTiles;
using ASD_project.World.Models.Interfaces;
using ASD_project.World.Models.TerrainTiles;
using ASD_project.World.Services;

namespace ASD_project.World
{
    public class NoiseMapGenerator : INoiseMapGenerator
    {
        private IFastNoise _worldNoise;
        private IFastNoise _itemNoise;
        private readonly int _seed;
        private IItemService _itemService;
        private List<ItemSpawnDTO> _items;

        [ExcludeFromCodeCoverage]
        public NoiseMapGenerator(int seed, IItemService itemService, List<ItemSpawnDTO> items)
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
            _seed = seed;
            _itemService = itemService;
            _items = items;
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
            return new Chunk(chunkX, chunkY, map, chunkRowSize, _seed);
        }

        private ITile CreateTileWithItemFromNoise(float worldNoise, float itemNoise, int x, int y)
        {
            var tile = GetTileFromNoise(worldNoise, x, y);
            
            if (!tile.IsAccessible)
            {
                return tile;
            }
            
            var item = _itemService.GenerateItemFromNoise(itemNoise, x, y);
            var itemSpawnDTO = new ItemSpawnDTO { Item = item, XPosition = x, YPosition = y };

            if (item == null)
                return tile;
            
            if (_items.Exists(itemInList => itemInList.Item.ItemId == item.ItemId))
            {
                  return tile;
            }
            _items.Add(itemSpawnDTO);
            tile.ItemsOnTile.Add(item);
            
            return tile;                      
        }

        private ITile GetTileFromNoise(float noise, int x, int y)
        {
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
    }
}