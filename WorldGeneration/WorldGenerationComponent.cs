using System;

namespace WorldGeneration
{
    public class Program
    {
        public Program()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            var map = new Map(4, 2243);
            map.DeleteMap();
            map.DisplayMap(3,0,30);
        }
    }
}