using System;

namespace WorldGeneration
{
    public class WorldGenerationComponent 
    {
        public WorldGenerationComponent(IList<IPlayer> players)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            var map = MapFactory.GenerateMap();
            map.DeleteMap();
            map.DisplayMap(3,0,30);
        }
    }
}