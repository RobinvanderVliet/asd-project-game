using System;
using System.Collections.Generic;
using System.Linq;
using Player.Model;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
{
    public class WorldGenerationComponent 
    {
        public WorldGenerationComponent(IList<IPlayer> players)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" En dan nu een random chunk met de seed " + 2243 + ": ");
            Console.ForegroundColor = ConsoleColor.Red;
            var world = new World(players, 2243);
            world.LoadAreaForPlayer(players.First(), 30);
            Console.ForegroundColor = ConsoleColor.White;
            var seed = new Random().Next(999999);
            Console.WriteLine(" En dan nu een random chunk met de seed " + seed + ": ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            var randomWorld = new World(players, seed);
            randomWorld.LoadAreaForPlayer(players.First(), 30);
            var db = new Database.Database();
            db.DeleteTileMap();
        }
    }
}