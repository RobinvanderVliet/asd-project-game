using System;
using System.Collections.Generic;
using System.Linq;
using WorldGeneration.Database;
using WorldGeneration.Models;
using WorldGeneration.Models.BuildingTiles;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.TerrainTiles;

namespace WorldGeneration
{
    public class Map
    {
        private List<Chunk> _chunks;
        private int _chunkSize;
        private int _seed;

        public Map(int chunkSize = 10, int seed = 0620520399)
        {
            this._chunkSize = chunkSize;
            this._seed = seed;
        }

        public void LoadArea(int[] playerLocation, int viewDistance)
        {
            var chunksWithinLoadingRange = CalculateChunksToLoad(playerLocation, viewDistance);
            var chunks = new List<Chunk>();
            var db = new Database.Database();
            
            foreach (var chunkXY in chunksWithinLoadingRange)
            {
                var chunk = db.GetChunk(chunkXY[0], chunkXY[1]);
                if (chunk == null)
                {
                    chunks.Add(GenerateNewChunk(chunkXY[0], chunkXY[1]));
                }
                else
                {
                    chunks.Add(db.GetChunk(chunkXY[0], chunkXY[1]));
                }
            }
            DisplayMap(0, 0, viewDistance, chunks);
        }

        public List<int[]> CalculateChunksToLoad(int[] playerLocation, int viewDistance)
        {
            var maxX = (playerLocation[0] + viewDistance * 2 + _chunkSize ) / _chunkSize;
            var minX = (playerLocation[0] - viewDistance * 2 - (_chunkSize * 2)) / _chunkSize; // chunks beginnen links bovenin, dus daarom *2
            var maxY = (playerLocation[1] + viewDistance * 2 + (_chunkSize * 2)) / _chunkSize;
            var minY = (playerLocation[1] - viewDistance * 2 - _chunkSize) / _chunkSize;
            var chunksWithinLoadingRange = new List<int[]>();
            

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    chunksWithinLoadingRange.Add(new int[]{x,y});
                }
            }
            return chunksWithinLoadingRange;
        }

        private void DisplayMap(int playerx, int playery, int viewDistance, List<Chunk> chunks)
        {
            for (int y = playery; y < (viewDistance * 2 + 1); y++)
            {
                for (int x = playerx; x < (viewDistance * 2 + 1); x++)
                {
                    var chunk = chunks.FirstOrDefault(chunk =>
                        chunk.X * _chunkSize <= x && chunk.X * _chunkSize > x - _chunkSize && chunk.Y * _chunkSize >= y && chunk.Y * _chunkSize < y + _chunkSize);
                    if(chunk == null)
                    {
                        throw new Exception("this chunk should not be null");
                    }
                    Console.Write(chunk.Map[chunk.GetPositionInTileArrayByWorldCoordinates(x, y)].Symbol);
                    if (x == viewDistance * 2)
                    {
                        Console.WriteLine("");
                    }
                }
            }
        }
        
        private Chunk GenerateNewChunk(int x, int y)
        {
            return NoiseMapGenerator.GenerateChunk(x, y, _chunkSize, _seed);
        }
    }
}       