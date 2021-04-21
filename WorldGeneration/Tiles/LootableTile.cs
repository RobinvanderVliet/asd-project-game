/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 3.
     
    Goal of this file: Supplies sub-classes with methods and variables of super-class and interface.
     
*/

using System;
using WorldGeneration.Tiles.Interfaces;

namespace WorldGeneration.Tiles
{
    class LootableTile : Tile, ILootableTile
    {
        public LootableTile(int X, int Y) : base(X, Y)
        {
        }

        public int GenerateLoot()
        {
            throw new NotImplementedException();
        }

        public void LootItem(int Item)
        {
            throw new NotImplementedException();
        }
    }
}
