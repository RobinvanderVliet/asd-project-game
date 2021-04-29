using System;
using System.Linq;
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

                //Console.Write(" " + Map[i].Symbol);
                Console.Write(" {"+GetTileCoordinatesInWorld(i).ToArray()[0]+ " " + GetTileCoordinatesInWorld(i).ToArray()[1]+"} ");
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
            //return (x % RowSize) + ((RowSize * RowSize) - (Y * RowSize - y) * RowSize);
            var y1 = (RowSize * RowSize - RowSize);
            //Console.Write(" Y1 = " + y1);
            var y2 = (Math.Abs((Y * RowSize - y) * RowSize) - RowSize);
            //Console.Write(", y2 = " + y2);
            Console.Write(" total: " + (x % RowSize + y2));
            return (x % RowSize) + (Math.Abs((Y * RowSize - y) * RowSize) - RowSize);
        }
        
        public ITile GetTileByWorldCoordinates(int x, int y)
        {
            return Map[GetPositionInTileArrayByWorldCoordinates(x,y)];
        }
    }
}