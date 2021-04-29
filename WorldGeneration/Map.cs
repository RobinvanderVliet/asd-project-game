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
        public List<Chunk> _chunks;
        private Database.Database _db;

        public Map(int chunkSize = 10, int seed = 0620520399, string dbLocation = "C:\\Temp\\ChunkDatabase.db", string dbCollectionName = "Chunks")
        {
            _chunkSize = chunkSize;
            _seed = seed;
            _db = new Database.Database(dbLocation, dbCollectionName);
        }

        public void LoadArea(int playerX, int playerY, int viewDistance)
        {
            var chunksWithinLoadingRange = CalculateChunksToLoad(playerX, playerY, viewDistance);
            _chunks = new List<Chunk>();

            var lastY = -1000;
            foreach (var chunkXY in chunksWithinLoadingRange)
            {
                if (chunkXY[0] > lastY)
                {
                    Console.WriteLine(" ");
                    lastY = chunkXY[0];
                }
                Console.Write(" {" + chunkXY[0] + " " + chunkXY[1] + "} ");
                
                var chunk = _db.GetChunk(chunkXY[0], chunkXY[1]);
                _chunks.Add(chunk == null
                    ? GenerateNewChunk(chunkXY[0], chunkXY[1])
                    : _db.GetChunk(chunkXY[0], chunkXY[1]));
            }
        }

        private IEnumerable<int[]> CalculateChunksToLoad(int playerX, int playerY, int viewDistance)
        {
            var maxX = (playerX + viewDistance * 2 + _chunkSize) / _chunkSize; 
            //playerX + viewDistance = viewscreen, viewscreen * 2 for buffer, + chunkSize for getting the nearest chunk, even if it's huge. / chunksize to get it to chunk coordinates
            var minX = (playerX - viewDistance * 2 - _chunkSize) / _chunkSize;
            var maxY = (playerY + viewDistance * 2 + _chunkSize) / _chunkSize + 1;
            var minY = (playerY - viewDistance * 2 - _chunkSize) / _chunkSize;
            var chunksWithinLoadingRange = new List<int[]>();
            Console.WriteLine("playerX = " + playerX + ", playerX = " + playerX + ", viewDistance = " + viewDistance);
            Console.WriteLine("maxChunkX = " + maxX + ", minChunkX = " + minX + ", maxChunkY = " + maxY + ", minChunkY = " + minY);
            Console.WriteLine("maxX = " + maxX * _chunkSize + ", minX = " + minX * _chunkSize + ", maxY = " + maxY * _chunkSize + ", minY = " + minY * _chunkSize);

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y < maxY; y++)
                {
                    chunksWithinLoadingRange.Add(new[] {x, y});
                }
            }
            return chunksWithinLoadingRange;
        }

        public void DisplayMap(int playerX, int playerY, int viewDistance)
        {
            Console.WriteLine("Starting point Y: " +  (playerY - viewDistance) + ", Ending point Y: " + ((playerY - viewDistance) + (viewDistance * 2)));
            Console.WriteLine("Starting point X: " +  (playerX - viewDistance) + ", Ending point X: " + ((playerX - viewDistance) + (viewDistance * 2)));
            for (var y = (playerY + viewDistance); y > ((playerY + viewDistance) - (viewDistance * 2)); y--)
            {
                for (var x = (playerX - viewDistance); x < ((playerX - viewDistance) + (viewDistance * 2)); x++)
                {
                    var chunk = GetChunkForTileXAndY(x, y);
                    //Console.Write(" [" + x +" " + y + " " + chunk.GetPositionInTileArrayByWorldCoordinates(x, y) + " " + chunk.X + " " + chunk.Y + "]");
                    Console.Write(" " + chunk.Map[chunk.GetPositionInTileArrayByWorldCoordinates(x, y)].Symbol);
                }
                Console.WriteLine("");
            }
        }

        public Chunk GenerateNewChunk(int chunkX, int chunkY)
        {
            var chunk = NoiseMapGenerator.GenerateChunk(chunkX, chunkY, _chunkSize, _seed);
            _db.InsertChunkIntoDatabase(chunk);
            return chunk;
        }

        public Chunk GetChunkForTileXAndY(int x, int y)
        {
            var chunk = _chunks.FirstOrDefault(chunk =>
                chunk.X * _chunkSize <= x 
                && chunk.X * _chunkSize > x - _chunkSize 
                && chunk.Y * _chunkSize >= y &&
                chunk.Y * _chunkSize < y + _chunkSize);
            
            if (chunk == null)
            {
                throw new Exception("Tried to find a chunk that has not been loaded");
            }
            return chunk;
        }

        public void DeleteMap()
        {
            _db.DeleteTileMap();
        }
    }
}