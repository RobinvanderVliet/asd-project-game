using System.Diagnostics.CodeAnalysis;
using ASD_project.World.Models;
using ASD_project.World.Models.HazardousTiles;
using ASD_project.World.Models.Interfaces;
using ASD_project.World.Models.TerrainTiles;
using ASD_project.World.Services;

namespace ASD_project.World
{
    public class NoiseMapGenerator : INoiseMapGenerator
    {
        private IFastNoise _noise;
        private IFastNoise _itemNoise;
        private readonly int _seed;
        private IItemService _itemService;

        [ExcludeFromCodeCoverage]
        public NoiseMapGenerator(int seed, IItemService itemService)
        {
            _noise = new FastNoiseLite();
            _noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
            _noise.SetFrequency(0.015f);
            _noise.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);
            _noise.SetSeed(seed);
            _seed = seed;
            _itemService = itemService;
            _itemNoise = new FastNoiseLite();
            _itemNoise.SetFrequency(1f);
            _itemNoise.SetCellularReturnType(FastNoiseLite.CellularReturnType.CellValue);
            _itemNoise.SetSeed(seed);
        }

        public Chunk GenerateChunk(int chunkX, int chunkY, int chunkRowSize)
        {
            var itemSpawner = new RandomItemGenerator();
            var map = new ITile[chunkRowSize * chunkRowSize];
            for (var y = 0; y < chunkRowSize; y++)
            {
                for (var x = 0; x < chunkRowSize; x++)
                {
                    map[y * chunkRowSize + x] = CreateTileFromNoise(_noise.GetNoise(x + chunkX * chunkRowSize, y + chunkY * chunkRowSize)
                        , x + chunkRowSize * chunkX
                        , chunkRowSize * chunkY - chunkRowSize + y);
                }
            }
            
            var createdChunk = new Chunk(chunkX, chunkY, map, chunkRowSize, _seed); 
            var modifiedChunk = itemSpawner.Spawn(createdChunk, _itemNoise.GetNoise(chunkX, chunkY));
            return modifiedChunk;
        }

        private ITile CreateTileFromNoise(float noise, int x, int y)
        {
            return GetTileFromNoise(noise, x, y);
        }

        public ITile GetTileFromNoise(float noise, int x, int y)
        {
            // this function is public for unit testing purposes only.
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

        public void SetNoise(IFastNoise noise)
        {
            // This function exists for unit testing purposes.
            _noise = noise;
        }
    }
}