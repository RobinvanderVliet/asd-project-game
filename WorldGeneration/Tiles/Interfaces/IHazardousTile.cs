/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 3.
     
    Goal of this file: Supplies interface for HazardousTile super-class.
     
*/

namespace WorldGeneration.Tiles.Interfaces
{
    interface IHazardousTile : ITile
    {
        int GetDamage(int Time);
    }
}
