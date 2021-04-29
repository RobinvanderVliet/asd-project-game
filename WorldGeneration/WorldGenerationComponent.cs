using System;

namespace WorldGeneration
{
    public class Program
    {
        public Program()
        {
            
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" En dan nu een random chunk met de seed " + 2243 + ": ");
            Console.ForegroundColor = ConsoleColor.Red;
            var map = new Map(10, 2243);
            map.DeleteMap();
            map.LoadArea(0, 0, 4);
            foreach (var chunk in map._chunks)
            {
                chunk.DisplayChunk();
            }
            
            map.DisplayMap(0,0,4);
            
            /*
            Console.ForegroundColor = ConsoleColor.White;
            var seed = new Random().Next(999999);
            Console.WriteLine(" En dan nu een random chunk met de seed " + seed + ": ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            var randomMap = new Map(16, seed);
            map.DeleteMap();
            randomMap.LoadArea(new[] {0, 0}, 30);*/
            
        }
    }
}