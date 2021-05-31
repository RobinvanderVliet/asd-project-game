using System;
using System.Collections.Generic;

namespace WorldGeneration
{
    public class World
    {
        private IMap _map;
        public Player CurrentPlayer { get; set; }
        private IList<Player> _players;
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

            if (_characters.Any(x => x.PlayerGuid.Equals(characterPositionDTO.PlayerGuid)))
            else
            {
                _characters.Where(x => x.PlayerGuid.Equals(characterPositionDTO.PlayerGuid)).FirstOrDefault().XPosition = characterPositionDTO.XPosition;
                _characters.Where(x => x.PlayerGuid.Equals(characterPositionDTO.PlayerGuid)).FirstOrDefault().YPosition = characterPositionDTO.YPosition;                
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
            _characters.Add(mapCharacterDto);
            _players.Add(player);
        }

        public void DisplayWorld()
        {
            if (CurrentPlayer is null || _characters is null)
            if (CurrentPlayer != null && _players != null)
            {
                Console.Clear();
                _map.DisplayMap(CurrentPlayer, _viewDistance, new List<Character>(_players));
            }
            _map.DisplayMap(CurrentPlayer, _viewDistance, _characters);
            
        }

        public void DeleteMap()
        {
            _map.DeleteMap();
        }
    }
}
     
