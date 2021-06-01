using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldGeneration
{
    public class World
    {
        private IMap _map;
        public Player CurrentPlayer { get; set; }
        private List<Player> _players;
        private readonly int _viewDistance;

        public World(int seed, int viewDistance, IMapFactory mapFactory)
        {
            _players = new ();
            _map = mapFactory.GenerateMap(seed);
            _viewDistance = viewDistance;
            DeleteMap();
        }

        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition)
        {
            if (CurrentPlayer.Id == userId)
            {
                CurrentPlayer.XPosition = newXPosition;
                CurrentPlayer.YPosition = newYPosition;
            }
            else
            {
                var player = _players.Find(x => x.Id == userId);
                player.XPosition = newXPosition;
                player.YPosition = newYPosition;
            }
            DisplayWorld();
        }

        public void AddPlayerToWorld(Player player, bool isCurrentPlayer)
        {
            if (isCurrentPlayer)
            {
                CurrentPlayer = player;
            }
            _players.Add(player);
        }

        public void DisplayWorld()
        {
            if (CurrentPlayer != null && _players != null)
            {
                _map.DisplayMap(CurrentPlayer, _viewDistance, _players);
            }
        }

        public void DeleteMap()
        {
            _map.DeleteMap();
        }
    }
}
     
