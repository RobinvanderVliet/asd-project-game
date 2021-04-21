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
    public class BuildingTile : Tile, IBuildingTile
    {
        public BuildingTile() 
        {
        }

        public void DrawBuilding()
        {
            throw new NotImplementedException();
        }
    }
}
