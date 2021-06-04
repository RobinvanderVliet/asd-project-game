using Creature.Creature;
using System.Collections.Generic;
using System.Numerics;
using WorldGeneration.Models.Interfaces;
using System.Linq;
using UserInterface;

namespace WorldGeneration
{
    public class World : IWorld
    {
        private IMap _map;
        public Player CurrentPlayer;
        public List<Player> _players { get; set; }
        public List<Character> _creatures { get; set; }
        public List<Character> movesList = new List<Character>();

        private readonly int _viewDistance;
        private IScreenHandler _screenHandler;

        public World(int seed, int viewDistance, IMapFactory mapFactory, IScreenHandler screenHandler)
        {
            _players = new();
            _creatures = new();
            _map = mapFactory.GenerateMap(seed);
            _viewDistance = viewDistance;
            _screenHandler = screenHandler;
            DeleteMap();
        }

        public void UpdateCharacterPosition(string id, int newXPosition, int newYPosition)
        {
            var player = _players.FirstOrDefault(x => x.Id == id);
            if (player != null)
            {
                player.XPosition = newXPosition;
                player.YPosition = newYPosition;
            }
            var creature = _creatures.FirstOrDefault(x => x.Id == id);
            if (creature != null)
            {
                creature.XPosition = newXPosition;
                creature.YPosition = newYPosition;
            }
            UpdateAI();
            UpdateMap();
        }

        public void AddPlayerToWorld(Player player, bool isCurrentPlayer = false)
        {
            if (isCurrentPlayer)
            {
                CurrentPlayer = player;
            }
            _players.Add(player);
            UpdateMap();
        }

        public void AddCreatureToWorld(Character character)
        {
            _creatures.Add(character);
            UpdateMap();
        }

        public void UpdateMap()
        {
            if (CurrentPlayer != null && _players != null && _creatures != null)
            {
                var characters = ((IEnumerable<Character>)_players).Concat(_creatures).ToList();
                //_screenHandler.UpdateWorld(_map.GetMapAroundCharacter(CurrentPlayer, _viewDistance, characters));
                _map.DisplayMap(CurrentPlayer, _viewDistance, characters);
            }
        }

        public char[,] GetMapAroundCharacter(Character character)
        {
            var characters = ((IEnumerable<Character>)_players).Concat(_creatures).ToList();
            return _map.GetMapAroundCharacter(character, _viewDistance, characters);
        }

        public void DeleteMap()
        {
            _map.DeleteMap();
        }

        public void UpdateAI()
        {
            movesList = new List<Character>();
            foreach (Character monster in _creatures)
            {
                if (monster is SmartMonster smartMonster)
                {
                    UpdateSmartMonster(smartMonster);
                }
            }
        }

        private void UpdateSmartMonster(SmartMonster smartMonster)
        {
            smartMonster.Update();
            movesList.Add(smartMonster);
        }

        public Player GetPlayer(string id)
        {
            return _players.Find(x => x.Id == id);
        }

        public ITile GetCurrentTile()
        {
            return _map.GetLoadedTileByXAndY(CurrentPlayer.XPosition, CurrentPlayer.YPosition);
        }

        public ITile GetTileForPlayer(Player player)
        {
            return _map.GetLoadedTileByXAndY(player.XPosition, player.YPosition);
        }
    }
}