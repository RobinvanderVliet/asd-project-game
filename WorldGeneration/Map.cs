using System;
using System.Collections.Generic;
using System.Linq;
using WorldGeneration.Models;

namespace WorldGeneration
{
    public class Map
    {
        private readonly int _chunkSize;
        private readonly int _seed;
        private IList<Chunk> _chunks;

        public Map(int chunkSize = 10, int seed = 0620520399)
        {
            _chunkSize = chunkSize;
            _seed = seed;
        }

        public void LoadArea(int[] playerLocation, int viewDistance)
        {
            var chunksWithinLoadingRange = CalculateChunksToLoad(playerLocation, viewDistance);
            var chunks = new List<Chunk>();
            var db = new Database.Database();

            foreach (var chunkXY in chunksWithinLoadingRange)
            {
                var chunk = db.GetChunk(chunkXY[0], chunkXY[1]);
                chunks.Add(chunk == null
                    ? GenerateNewChunk(chunkXY[0], chunkXY[1])
                    : db.GetChunk(chunkXY[0], chunkXY[1]));
            }

            DisplayMap(0, 0, viewDistance, chunks);
        }

        private IEnumerable<int[]> CalculateChunksToLoad(int[] playerLocation, int viewDistance)
        {
            var maxX = (playerLocation[0] + viewDistance * 2 + _chunkSize) / _chunkSize;
            var minX = (playerLocation[0] - viewDistance * 2 - _chunkSize * 2) /
                       _chunkSize; // chunks beginnen links bovenin, dus daarom *2
            var maxY = (playerLocation[1] + viewDistance * 2 + _chunkSize * 2) / _chunkSize;
            var minY = (playerLocation[1] - viewDistance * 2 - _chunkSize) / _chunkSize;
            var chunksWithinLoadingRange = new List<int[]>();


            for (var x = minX; x <= maxX; x++)
            for (var y = minY; y < maxY; y++)
                chunksWithinLoadingRange.Add(new[] {x, y});
            return chunksWithinLoadingRange;
        }

        private void DisplayMap(int playerx, int playery, int viewDistance, IReadOnlyCollection<Chunk> chunks)
        {
            for (var y = playery; y < viewDistance * 2 + 1; y++)
            {
                for (var x = playerx; x < viewDistance * 2 + 1; x++)
                {
                    var chunk = chunks.FirstOrDefault(chunk =>
                        chunk.X * _chunkSize <= x 
                        && chunk.X * _chunkSize > x - _chunkSize 
                        && chunk.Y * _chunkSize >= y 
                        && chunk.Y * _chunkSize < y + _chunkSize);
                    if (chunk == null) throw new Exception("this chunk should not be null");
                    Console.Write(" " + chunk.Map[chunk.GetPositionInTileArrayByWorldCoordinates(x, y)].Symbol);
                    if (x == viewDistance * 2) Console.WriteLine("");
                }
            }
        }

        private Chunk GenerateNewChunk(int x, int y)
        {
            var chunk = NoiseMapGenerator.GenerateChunk(x, y, _chunkSize, _seed);
            new Database.Database().InsertChunkIntoDatabase(chunk);
            return chunk;
        }
    }
}