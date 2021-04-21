/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 3.
     
    Goal of this file: Supplies sub-classes with methods and variables of super-class and interface.
     
*/

using WorldGeneration.Tiles.Interfaces;

namespace WorldGeneration.Tiles
{
    abstract class Tile : ITile
    {
        public int X { get; set; }
        public int Y { get; set; }

        public string Symbol { get; set; }
        public bool Accessible { get; set; }

        public Tile(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

    }
}
