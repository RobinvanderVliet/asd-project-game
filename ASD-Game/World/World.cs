using System.Collections.Generic;
using System.Linq;
using UserInterface;

namespace WorldGeneration
{
    public class World : IWorld
    {
        private IMap _map;
        public Player CurrentPlayer;
        private List<Player> _players;
        private List<Creature> _creatures;
        private readonly int _viewDistance;
        private IScreenHandler _screenHandler;

        public World(int seed, int viewDistance, IMapFactory mapFactory, IScreenHandler screenHandler)
        {
            _players = new ();
            _creatures = new ();
            _map = mapFactory.GenerateMap(seed);
            _viewDistance = viewDistance;
            _screenHandler = screenHandler;
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
        
        public void AddCreatureToWorld(Creature creature)
        {
            _creatures.Add(creature);
            UpdateMap();
        }

        public void UpdateMap()
        {
            if (CurrentPlayer != null && _players != null && _creatures != null)
            {
                var characters = ((IEnumerable<Character>)_players).Concat(_creatures).ToList();
                //_screenHandler.UpdateWorld(_map.GetMapAroundCharacter(CurrentPlayer, _viewDistance, characters));
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
    }
}
     
