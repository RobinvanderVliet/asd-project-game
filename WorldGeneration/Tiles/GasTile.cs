/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].
 
    This file is created by team: 3.
     
    Goal of this file: Data object for tile properties.
     
*/

namespace WorldGeneration.Tiles
{
    public class GasTile : HazardousTile
    {
        public int Radius { get; set; }
        public GasTile(int Radius) 
        {
            Symbol = "&";
            Accessible = true;

            this.Radius = Radius;
        }

        public override int GetDamage(int time)
        {
            return time * Radius;
        }
    }
}
