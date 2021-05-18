using System.Collections.Generic;
using System.Linq;
using Player.DTO;
using Player.Model;
using WorldGeneration.Models.Interfaces;

namespace WorldGeneration
{
    public class World
    {
        private Map _map;
        private IPlayerModel _currentPlayer { get; set; }
        public IList<PlayerDTO> _players { get; set; }
        public IList<ICharacter> _characters { get; set; }

        public World(IList<PlayerDTO> players, IPlayerModel currentPlayer, IList<ICharacter> characters, int seed)
        {
            _players = players;
            _map = MapFactory.GenerateMap(seed: seed);
            _currentPlayer = currentPlayer;
            _characters = characters;
        }
        
        public void UpdatePlayerPosition(PlayerDTO playerDto)
        {
            foreach (var player in _players.Where(player => playerDto.PlayerName == player.PlayerName))
            {
                player.X = playerDto.X;
                player.Y = playerDto.Y;
            }
        }

        public void DisplayWorld(int viewDistance)
        {
            // TOTO: Add characters 
            _map.DeleteMap();
            _map.DisplayMap(_currentPlayer, viewDistance, _players, _characters);
        }
    }
}
     
