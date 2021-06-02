using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseHandler.Services;
using Display;
using WorldGeneration.Helper;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
{
    public class Map : IMap
    {
        private readonly int _chunkSize;
        private IList<Chunk> _chunks; // NOT readonly, don't listen to the compiler
        private readonly IDatabaseService<Chunk> _chunkDBService;
        private ChunkHelper _chunkHelper;
        private readonly INoiseMapGenerator _noiseMapGenerator;
        private int _seed;

        public Map(
            INoiseMapGenerator noiseMapGenerator
            , int chunkSize
            , IConsolePrinter consolePrinter
            , IDatabaseService<Chunk> chunkDbServices
            , int seed
            , IList<Chunk> chunks = null
        )
        {
            if (chunkSize < 1)
            {
                throw new InvalidOperationException("Chunksize smaller than 1.");
            }
            _chunkSize = chunkSize;
            _chunks = chunks ?? new List<Chunk>();
            _noiseMapGenerator = noiseMapGenerator;
            _chunkDBService = chunkDbServices;
            _seed = seed;
        }

        // checks if there are new chunks that have to be loaded
        private void LoadArea(int playerX, int playerY, int viewDistance) {
            var chunksWithinLoadingRange = CalculateChunksToLoad(playerX, playerY, viewDistance);
            foreach (var chunkCoordinates in chunksWithinLoadingRange)
            {
                if (_chunks.Any(chunk => chunk.X == chunkCoordinates[0] && chunk.Y == chunkCoordinates[1])) continue;
                {
                    // chunk isn't loaded in local memory yet
                    var chunk = new Chunk { 
                        X = chunkCoordinates[0], 
                        Y = chunkCoordinates[1] 
                    };
                    var getAllChunksQuery = _chunkDBService.GetAllAsync();
                    getAllChunksQuery.Wait();
                    var results = getAllChunksQuery.Result.FirstOrDefault(c => c.X == chunkCoordinates[0] && c.Y == chunkCoordinates[1] && c.Seed == _seed);
                    if (results == null)
                    {
                        _chunks.Add(GenerateNewChunk(chunkCoordinates[0], chunkCoordinates[1]));
                    }
                    else
                    {
                        _chunks.Add(results);
                    }
                }
            }
        }

        private List<int[]> CalculateChunksToLoad(int playerX, int playerY, int viewDistance)
        {
            // viewDistance * 2 is to get a full screen
            // , + playerX to get to the right location
            // , + chunk size to add some loading buffer
            // , / chunk size to convert tile coordinates to world coordinates
            // same for the other variables
            var maxX = (playerX + viewDistance * 2 + _chunkSize) / _chunkSize; 
            var minX = (playerX - viewDistance * 2 - _chunkSize) / _chunkSize;
            var maxY = (playerY + viewDistance * 2 + _chunkSize) / _chunkSize + 1;
            var minY = (playerY - viewDistance * 2 - _chunkSize) / _chunkSize;
            var chunksWithinLoadingRange = new List<int[]>();

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y < maxY; y++)
                {
                    chunksWithinLoadingRange.Add(new[] {x, y});
                }
            }
            return chunksWithinLoadingRange;
        }
        
        public char[,] GetMapAroundCharacter(Player centerCharacter, int viewDistance, List<Character> allCharacters)
        {
            if (viewDistance < 0)
            {
                throw new InvalidOperationException("viewDistance smaller than 0.");
            }
            
            var tileArray = new char[viewDistance * 2 + 1, viewDistance * 2 + 1];
            var centerCharacterXPosition = centerCharacter.XPosition;
            var centerCharacterYPosition = centerCharacter.YPosition;
            LoadArea(centerCharacterXPosition, centerCharacterYPosition, viewDistance);
            
            for (var y = tileArray.GetLength(0) - 1; y >= 0; y--)
            {
                for (var x = 0; x < tileArray.GetLength(1); x++)
                {
                    var tile = GetLoadedTileByXAndY(x + (centerCharacterXPosition - viewDistance), (centerCharacterYPosition + viewDistance) - y);
                    tileArray[y, x] = GetDisplaySymbol(tile, allCharacters).ToCharArray()[0];
                }
            }
            return tileArray;
        }
        
        private string GetDisplaySymbol(ITile tile, List<Character> characters)
        {
            var characterOnTile = characters.FirstOrDefault(character => character.XPosition == tile.XPosition && character.YPosition - 1 == tile.YPosition);
            if(characterOnTile != null)
            {
                return characterOnTile.Symbol;
            }
            else
            {
                return tile.Symbol;
            }
        }

        private Chunk GenerateNewChunk(int chunkX, int chunkY)
        {
            var chunk = _noiseMapGenerator.GenerateChunk(chunkX, chunkY, _chunkSize);
            _chunkDBService.CreateAsync(chunk);
            return chunk;
        }

        private Chunk GetChunkForTileXAndY(int x, int y)
        {
            var chunk = _chunks.FirstOrDefault(chunk =>
                chunk.X * _chunkSize <= x 
                && chunk.X * _chunkSize > x - _chunkSize 
                && chunk.Y * _chunkSize >= y &&
                chunk.Y * _chunkSize < y + _chunkSize);
            return chunk;
        }
        
        public void DeleteMap()
        {
            _chunks.Clear();
            _chunkDBService.DeleteAllAsync();
        }
        
        private ITile GetLoadedTileByXAndY(int x, int y)
        {
            _chunkHelper = new ChunkHelper(GetChunkForTileXAndY(x, y));
            return _chunkHelper.GetTileByWorldCoordinates(x, y);
        }

    }
}