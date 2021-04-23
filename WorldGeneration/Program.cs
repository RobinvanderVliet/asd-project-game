using System;

namespace WorldGeneration
{
    public class Program
    {
        public Program()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            var map = new Map(100, 2243);
            map.LoadArea(new[] {0, 0}, 30);
            var db = new Database.Database();
            db.DeleteTileMap();
        }
    }
}