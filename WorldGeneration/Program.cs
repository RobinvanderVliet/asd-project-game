using System;
using System.Collections.Specialized;
using WorldGeneration.Models;
using WorldGeneration.Models.BuildingTiles;
using WorldGeneration.Models.Interfaces;
using WorldGeneration.Models.TerrainTiles;

namespace WorldGeneration
{
    public class Program
    {
        public Program()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            var map = new Map(100, seed:2243);
            map.LoadArea(new []{0,0}, 30);
            var db = new Database.Database();
            db.DeleteTileMap();
            
            
        }
    }
}