using System;
using ASD_Game.World.Models;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World.Helpers
{
    public class ChunkHelper
    {
        public Chunk Chunk { get; set; }
        public ChunkHelper(Chunk chunk)
        {
            this.Chunk = chunk;
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

