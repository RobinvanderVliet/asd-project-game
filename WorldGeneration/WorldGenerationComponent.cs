using System;

namespace WorldGeneration
{
    public class Program
    {
        public Program()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            var map = MapFactory.GenerateMap();
            map.DeleteMap();
            map.DisplayMap(3,0,30);
        }
    }
}