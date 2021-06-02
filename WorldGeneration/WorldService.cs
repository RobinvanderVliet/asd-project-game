using System;
using System.Collections.Generic;

namespace WorldGeneration
{
    public class WorldService : IWorldService
    {
        private World _world;

        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition)
        {
            _world.UpdateCharacterPosition(userId, newXPosition, newYPosition);
        }

        public void AddPlayerToWorld(Player player, bool isCurrentPlayer)
        {
            _world.AddPlayerToWorld(player, isCurrentPlayer);
        }

        public void DisplayWorld()
        {
            _world.DisplayWorld();
        }
        
        public void DeleteMap()
        {
            _world.deleteMap();
        }

        public void GenerateWorld(int seed)
        {
            _world = new World(seed, 6);
        }

        public Player getCurrentPlayer()
        {
            return _world.CurrentPlayer;
        }

        public List<Player> getAllPlayers()
        {
            return _world._players;
        }

        public void playerDied(Player player)
        {
            player.Symbol = "X";
            
        }

        public bool isDead(Player player)
        {
            return player.Health <= 0;
        }
    }
}