/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 3.
     
    Goal of this file: Supplies sub-classes with methods and variables of super-class and interface.
     
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.Tiles.Interfaces;

namespace WorldGeneration.Tiles
{
    class BuildingTile : Tile, IBuildingTile
    {
        public BuildingTile(int X, int Y) : base(X, Y)
        {
        }

        public void DrawBuilding()
        {
            throw new NotImplementedException();
        }
    }
}
