/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 3.
     
    Goal of this file: Supplies sub-classes with methods and variables of super-class and interface.
     
*/

using WorldGeneration.Tiles.Interfaces;

namespace WorldGeneration.Tiles
{
    public abstract class HazardousTile : Tile, IHazardousTile
    {
        public int Damage { get; set; }

        public HazardousTile() 
        {

        }

        public abstract int GetDamage(int Time);
    }
}
