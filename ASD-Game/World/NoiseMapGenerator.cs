using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.Items.Services;
using ASD_Game.World.Models;
using ASD_Game.World.Models.HazardousTiles;
using ASD_Game.World.Models.Interfaces;
using ASD_Game.World.Models.TerrainTiles;
using WorldGeneration;
using WorldGeneration.Models.Enums;

namespace ASD_Game.World
{
    public class NoiseMapGenerator : INoiseMapGenerator
    {
        private IFastNoise _worldNoise;
        private IFastNoise _itemNoise;
        private IFastNoise _roadNoise;
        private IItemService _itemService;
        private List<ItemSpawnDTO> _items;
        private RoadPresetFactory _roadPresetFactory;
        private int _chunkRowSize;
        private bool _generateRoads;

        public NoiseMapGenerator(int seed, IItemService itemService, List<ItemSpawnDTO> items, int chunkRowSize)
        {
            _worldNoise = SetupNoise(0.015f, seed);
            _itemNoise = SetupNoise(10f,seed);
            _roadNoise = SetupNoise(10f, seed);
            _itemService = itemService;
            _items = items;
            _roadPresetFactory = new RoadPresetFactory(chunkRowSize);
            _chunkRowSize = chunkRowSize;
            _generateRoads = true;
        }

        public Chunk GenerateChunk(int chunkX, int chunkY)
        {
            var map = new ITile[_chunkRowSize * _chunkRowSize];
            // check which edges of the chunk will have roads.
           CompassDirections compassDirection = GetCompassDirections(chunkX, chunkY);
           var roadmap = _roadPresetFactory.GetRoadPreset(compassDirection);
            
            for (var y = 0; y < _chunkRowSize; y++)
            {
                for (var x = 0; x < _chunkRowSize; x++)
                {
                    if (roadmap[y * _chunkRowSize + x] != null && _generateRoads)
                    {
                        map[y * _chunkRowSize + x] = new StreetTile(x+ _chunkRowSize * chunkX
                            , _chunkRowSize * chunkY - _chunkRowSize + y);
                    }
                    else
                    {

                        map[y * _chunkRowSize + x] = CreateTileWithItemFromNoise(
                            _worldNoise.GetNoise(x + chunkX * _chunkRowSize, y + chunkY * _chunkRowSize)
                            , _itemNoise.GetNoise(x + chunkX * _chunkRowSize, y + chunkY * _chunkRowSize)
                            , x + _chunkRowSize * chunkX
                            , _chunkRowSize * chunkY - _chunkRowSize + y);
                    }
                }
            }
            return new Chunk(chunkX, chunkY, map, _chunkRowSize);
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
                (< 8) => new DirtTile(x, y), // This used to generate StreetTile, but those should no longer be randomly generated.
                _ => new GasTile(x, y)
            };
        }
        
        private CompassDirections GetCompassDirections (int chunkX, int chunkY)
        {
            // Step 1: generate four binaries based on the compass directions.
            // These use the coordinate of the current chunk and the adjacent chunk, so when the calculation is repeated in the adjacent chunk it will result in the same result.
            var north = GetBinaryForRoads(chunkY + chunkY + 1);
            var south = GetBinaryForRoads(chunkY + chunkY - 1);
            var east = GetBinaryForRoads(chunkX + chunkX + 1);
            var west = GetBinaryForRoads(chunkX + chunkX - 1);
            
            // Step 2: based on the previous binaries, a switch case is used to return the correct compass direction.
            switch (north, south, east, west)
            {
                case (0, 0, 0, 0):
                    return CompassDirections.NoRoads;
                case (1, 0, 0, 0):
                    return CompassDirections.NorthOnly;
                case (0, 1, 0, 0):
                    return CompassDirections.SouthOnly;
                case (0, 0, 1, 0):
                    return CompassDirections.EastOnly;
                case (0, 0, 0, 1):
                    return CompassDirections.WestOnly;
                case (1, 1, 0, 0):
                    return CompassDirections.NorthToSouth;
                case (1, 0, 1, 0):
                    return CompassDirections.NorthToEast;
                case (1, 0, 0, 1):
                    return CompassDirections.NorthToWest;
                case (0, 1, 1, 0):
                    return CompassDirections.EastToSouth;
                case (0, 1, 0, 1):
                    return CompassDirections.EastToWest;
                case (0, 0, 1, 1):
                    return CompassDirections.NorthToSouth;
                case (1, 1, 1, 0):
                    return CompassDirections.AllButWest;
                case (1, 1, 0, 1):
                    return CompassDirections.AllButEast;
                case (1, 0, 1, 1):
                    return CompassDirections.AllButSouth;
                case (0, 1, 1, 1):
                    return CompassDirections.AllButNorth;
                case (1, 1, 1, 1):
                    return CompassDirections.AllRoads;
                default:
                    return CompassDirections.NoRoads;
            }
        }

        private int GetBinaryForRoads(int combinedCoordinates)
        {
            var noiseResult = _roadNoise.GetNoise(combinedCoordinates, combinedCoordinates) * 10;
            return (int) (noiseResult % 2);
        }
        
        [ExcludeFromCodeCoverage]
        public void SetNoiseForUnitTests (IFastNoise noise)
        {
            _itemNoise = noise;
            _worldNoise = noise;
            _roadNoise = noise;
        }

        [ExcludeFromCodeCoverage]
        public void TurnOffRoadGeneration()
        {
            _generateRoads = false;
        }

        private IFastNoise SetupNoise(float frequency, int seed)
        {
            var noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            noise.SetFrequency(frequency);
            noise.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);
            noise.SetSeed(seed);
            return noise;
        }
    }
}