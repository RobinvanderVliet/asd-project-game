using System;
using System.Collections.Generic;
using WorldGeneration.Models.Interfaces;
using System.Linq;
using UserInterface;

namespace WorldGeneration
{
    public class World
    {
        private Map _map;
        public Player CurrentPlayer { get; set; }
        private List<Player> _players;
        private readonly int _viewDistance;
        private readonly IScreenHandler _screenHandler;

        public World(int seed, int viewDistance, IScreenHandler screenHandler)
        {
            _players = new ();
            _map = MapFactory.GenerateMap(seed: seed);
            _viewDistance = viewDistance;
            _screenHandler = screenHandler;
            DeleteMap();
        }

        public Player GetPlayer(string id)
        {
            return _players.Find(x => x.Id == id);
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
                var player = GetPlayer(userId);
                player.XPosition = newXPosition;
                player.YPosition = newYPosition;
            }
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
                _screenHandler.UpdateWorld(_map.GetMapAroundCharacter(CurrentPlayer, _viewDistance, new List<Character>(_players)));
            }
        }

        public void DeleteMap()
        {
            _map.DeleteMap();
        }

        public ITile GetLoadedTileByXAndY(int x, int y)
        {
            return _map.GetLoadedTileByXAndY(x, y);
        }
        
        public bool CheckIfPlayerOnTile(ITile tile)
        {
            if (_players.Exists(player => player.XPosition == tile.XPosition && player.YPosition == tile.YPosition))
            {
                return true;
            }

            return false;
        }
        
        public void LoadArea(int playerX, int playerY, int viewDistance)
        {
            _map.LoadArea(playerX, playerY, viewDistance);
        }
        
        public ITile GetCurrentTile()
        {
            return _map.GetLoadedTileByXAndY(CurrentPlayer.XPosition, CurrentPlayer.YPosition);
        }

        public ITile GetTileForPlayer(Player player)
        {
            return _map.GetLoadedTileByXAndY(player.XPosition, player.YPosition);
        }

        public List<Player> GetAllPlayers()
        {
            return _players;
        }
    }
}

