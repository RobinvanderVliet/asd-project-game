using System;
using System.Linq;
using Player.Model;

namespace WorldGeneration
{
    public class WorldGenerationComponent 
    {
        public WorldGenerationComponent(PlayerModel[] playerModel)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" En dan nu een random chunk met de seed " + 2243 + ": ");
            Console.ForegroundColor = ConsoleColor.Red;
            var world = new World(120, 2243);
            world.LoadAreaForPlayer(playerModel.First().CurrentPosition, 30);
            Console.ForegroundColor = ConsoleColor.White;
            var seed = new Random().Next(999999);
            Console.WriteLine(" En dan nu een random chunk met de seed " + seed + ": ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            var randomWorld = new World(120, seed);
            randomWorld.LoadAreaForPlayer(playerModel.First().CurrentPosition, 30);
            
            var db = new Database.Database();
            db.DeleteTileMap();
        }
    }
}