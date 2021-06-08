using System;
using WorldGeneration.Models;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Helper
{
    public class ChunkHelper
    {
        public Chunk Chunk { get; set; }

        public ChunkHelper(Chunk chunk)
        {
            Chunk = chunk;
        }

        public int[] GetTileCoordinatesInChunk(int indexInArray)
        {
            var x = indexInArray % Chunk.RowSize;
            var y = (int)Math.Floor((double)indexInArray / Chunk.RowSize);
            return new[] { x, y };
        }

        private int GetPositionInTileArrayByWorldCoordinates(int x, int y)
        {
            var yPos = Math.Abs(y);
            var chunkYPos = Math.Abs(Chunk.Y);
            while (x < 0)
            {
                x = x + Chunk.RowSize;
            }
            var y1 = (Chunk.RowSize * Chunk.RowSize - Chunk.RowSize) - (Math.Abs(chunkYPos * Chunk.RowSize - yPos) * Chunk.RowSize);
            var x1 = x % Chunk.RowSize;

            return x1 + y1;
        }

        public ITile GetTileByWorldCoordinates(int x, int y) => Chunk.Map[GetPositionInTileArrayByWorldCoordinates(x, y)];
    }
}