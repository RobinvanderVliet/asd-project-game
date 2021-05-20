using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransfer.POCO.World;
using DataTransfer.POCO.World.Interfaces;

namespace WorldGeneration.Services
{
    public class ChunkService
    {
        private Chunk _chunk;
        public ChunkService(Chunk chunk)
        {
            _chunk = chunk;
        }
        public int[] GetTileCoordinatesInChunk(int indexInArray)
        {
            var x = indexInArray % _chunk.RowSize;
            var y = (int)Math.Floor((double)indexInArray / _chunk.RowSize);
            return new[] { x, y };
        }

      /*  // returns the coordinates relative to the center of the world. First value is x, second is y.
        private int[] GetTileCoordinatesInWorld(int indexInArray)
        {
            var internalCoordinates = GetTileCoordinatesInChunk(indexInArray);
            return new[] { internalCoordinates[0] + _chunk.RowSize * _chunk.X, internalCoordinates[1] + _chunk.RowSize * _chunk.Y };
        }*/


        public int GetPositionInTileArrayByWorldCoordinates(int x, int y)
        {

            var yPos = Math.Abs(y);
            var chunkYPos = Math.Abs(_chunk.Y);
            while (x < 0)
            {
                x = x + _chunk.RowSize;
            }
            var y1 = (_chunk.RowSize * _chunk.RowSize - _chunk.RowSize) - (Math.Abs(chunkYPos * _chunk.RowSize - yPos) * _chunk.RowSize);
            var x1 = x % _chunk.RowSize;

            return x1 + y1;
        }

        public ITile GetTileByWorldCoordinates(int x, int y) => _chunk.Map[GetPositionInTileArrayByWorldCoordinates(x, y)];
    }
}

