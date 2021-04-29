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
            var map = new Map(4, 2243);
            map.DeleteMap();
            map.LoadArea(3, -1, 20);
            map.DisplayMap(3,-1,20);
            
            
            /*
            Console.ForegroundColor = ConsoleColor.White;
            var seed = new Random().Next(999999);
            Console.WriteLine(" En dan nu een random chunk met de seed " + seed + ": ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            var randomMap = new Map(16, seed);
            map.DeleteMap();
            randomMap.LoadArea(new[] {0, 0}, 30);*/
            
            //backToPrototyping(10);
        }

        public void backToPrototyping(int chunkSize)
        {
            var map = new Map(chunkSize);
            var chunkX = -1;
            var chunkY = -5;
            var chunk = map.GenerateNewChunk(chunkX, chunkY);
            Console.WriteLine("chunk linesize: " + chunk.Map.Length);
            for (var y = chunkSize * chunkY + chunkSize; y > chunkSize * chunkY; y--)
            {
                for (var x = chunkX * chunkSize; x < chunkX * chunkSize + chunkSize; x++)
                {
                    //chunk.GetPositionInTileArrayByWorldCoordinates(x, y);
                    Console.Write(" [" + x +" " + y + " " + chunk.GetPositionInTileArrayByWorldCoordinates(x, y) + " " + chunk.X + " " + chunk.Y + "]");
                    //Console.Write(" " + chunk.Map[chunk.GetPositionInTileArrayByWorldCoordinates(x, y)].Symbol);
                }
                Console.WriteLine("");
            }
        }
    }
}