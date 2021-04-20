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
        public int x { get; set; }
        public int y { get; set; }
        public Tile[] map { get; set; }
        public int rowSize { get; set; }

        public Chunk(int x, int y, Tile[] map, int rowSize)
        {
            this.x = x;
            this.y = y;
            this.map = map;
            this.rowSize = rowSize;
        }

        public Chunk()
        {
        }

        //writes out the symbols for the tilemap of the current chunk in the proper shape.
        public void displayChunk()
        {
            var rowNumber = 0;
            for (int i = 0; i < map.Length; i++)
            {
                if (i % rowSize == 0)
                {
                    Console.WriteLine(" ");
                }

                Console.Write(" " + map[i].tileType.symbol);
            }

            Console.WriteLine("");
        }

        //returns the coordinates relative to the start (left top) of the chunk. 0,0 is the left top. First value is x, second is y.
        public int[] getTileCoordinatesInChunk(int indexInArray)
        {
            var x = (indexInArray % rowSize);
            var y = (int) Math.Floor((double) indexInArray / rowSize);
            return new[] {x, y};
        }

        //returns the coordinates relative to the center of the world. First value is x, second is y.
        public int[] getTileCoordinatesInWorld(int indexInArray)
        {
            var internalCoordinates = getTileCoordinatesInChunk(indexInArray);
            return new[] {internalCoordinates[0] + rowSize * x, internalCoordinates[1] + rowSize * y};
        }
    }
}