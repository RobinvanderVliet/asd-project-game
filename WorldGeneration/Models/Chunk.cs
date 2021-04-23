using System;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration.Models
{
    public class Chunk
    {
        public Chunk(int x, int y, ITile[] map, int rowSize)
        {
            X = x;
            Y = y;
            Map = map;
            RowSize = rowSize;
        }

        public Chunk()
        {
        }

        public int X { get; set; }
        public int Y { get; set; }
        public ITile[] Map { get; set; }
        public int RowSize { get; set; }

        //writes out the symbols for the tilemap of the current chunk in the proper shape.
        public void DisplayChunk()
        {
            for (var i = 0; i < Map.Length; i++)
            {
                if (i % RowSize == 0) Console.WriteLine(" ");

                Console.Write(" " + Map[i].Symbol);
            }

            Console.WriteLine("");
        }

        //returns the coordinates relative to the start (left top) of the chunk. 0,0 is the left top. First value is x, second is y.
        public int[] GetTileCoordinatesInChunk(int indexInArray)
        {
            var x = indexInArray % RowSize;
            var y = (int) Math.Floor((double) indexInArray / RowSize);
            return new[] {x, y};
        }

        //returns the coordinates relative to the center of the world. First value is x, second is y.
        public int[] GetTileCoordinatesInWorld(int indexInArray)
        {
            var internalCoordinates = GetTileCoordinatesInChunk(indexInArray);
            return new[] {internalCoordinates[0] + RowSize * X, internalCoordinates[1] + RowSize * Y};
        }

        public int GetPositionInTileArrayByWorldCoordinates(int x, int y)
        {
            // return (x % RowSize) + (Y * RowSize + (y - Y * RowSize) * RowSize);
            return x % RowSize + (Y * RowSize - y) * RowSize;
        }
    }
}