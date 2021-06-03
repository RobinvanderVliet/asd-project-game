using System;
using ASD_project.World.Models;
using ASD_project.World.Models.Interfaces;

namespace ASD_project.World.Helpers
{
    public class ChunkHelper
    {
        public Chunk chunk { get; set; }
        public ChunkHelper(Chunk chunk)
        {
            this.chunk = chunk;
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

