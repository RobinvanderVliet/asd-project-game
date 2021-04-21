/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD project.
 
    This file is created by team: 3.
     
    Goal of this file: Storing Chunk data and executing functions.
     
*/

using System;

namespace WorldGeneration
{
    public class Chunk
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Tile[] Map { get; set; }
        public int RowSize { get; set; }

        public Chunk(int x, int y, Tile[] map, int rowSize)
        {
            this.X = x;
            this.Y = y;
            this.Map = map;
            this.RowSize = rowSize;
        }

        public Chunk()
        {
        }

        //writes out the symbols for the tilemap of the current chunk in the proper shape.
        public void DisplayChunk()
        {
            for (int i = 0; i < Map.Length; i++)
            {
                if (i % RowSize == 0)
                {
                    Console.WriteLine(" ");
                }

                Console.Write(" " + Map[i].TileType.Symbol);
            }

            Console.WriteLine("");
        }

        //returns the coordinates relative to the start (left top) of the chunk. 0,0 is the left top. First value is x, second is y.
        public int[] GetTileCoordinatesInChunk(int indexInArray)
        {
            var x = (indexInArray % RowSize);
            var y = (int) Math.Floor((double) indexInArray / RowSize);
            return new[] {x, y};
        }

        //returns the coordinates relative to the center of the world. First value is x, second is y.
        public int[] GetTileCoordinatesInWorld(int indexInArray)
        {
            var internalCoordinates = GetTileCoordinatesInChunk(indexInArray);
            return new[] {internalCoordinates[0] + RowSize * X, internalCoordinates[1] + RowSize * Y};
        }
    }
}