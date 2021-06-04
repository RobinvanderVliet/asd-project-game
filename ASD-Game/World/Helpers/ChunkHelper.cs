using System;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Helper
{
    public class ChunkHelper
    {
        public Chunk chunk { get; set; }

        public ChunkHelper(Chunk chunk)
        {
            this.chunk = chunk;
        }

        public int[] GetTileCoordinatesInChunk(int indexInArray)
        {
            var x = indexInArray % chunk.RowSize;
            var y = (int)Math.Floor((double)indexInArray / chunk.RowSize);
            return new[] { x, y };
        }

        private int GetPositionInTileArrayByWorldCoordinates(int x, int y)
        {
            var yPos = Math.Abs(y);
            var chunkYPos = Math.Abs(chunk.Y);
            while (x < 0)
            {
                x = x + chunk.RowSize;
            }
            var y1 = (chunk.RowSize * chunk.RowSize - chunk.RowSize) - (Math.Abs(chunkYPos * chunk.RowSize - yPos) * chunk.RowSize);
            var x1 = x % chunk.RowSize;

            return x1 + y1;
        }

        public ITile GetTileByWorldCoordinates(int x, int y) => chunk.Map[GetPositionInTileArrayByWorldCoordinates(x, y)];
    }
}