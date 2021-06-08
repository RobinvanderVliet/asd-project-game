using Creature.Creature;
using System.Collections.Generic;
using System.IO;
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
        private readonly IScreenHandler _screenHandler;
        private static readonly char _separator = Path.DirectorySeparatorChar;

        public World(int seed, int viewDistance, IMapFactory mapFactory, IScreenHandler screenHandler)
        {
            _players = new();
            _creatures = new();
            var currentDirectory = Directory.GetCurrentDirectory();
            _map = MapFactory.GenerateMap(dbLocation: $"Filename={currentDirectory}{_separator}ChunkDatabase.db;connection=shared;", seed: seed);
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
            else if (GetPlayer(userId) != null)
            {
                var player = GetPlayer(userId);
                player.XPosition = newXPosition;
                player.YPosition = newYPosition;
            }
            var creature = _creatures.FirstOrDefault(x => x.Id == userId);
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
                _screenHandler.UpdateWorld(_map.GetMapAroundCharacter(CurrentPlayer, _viewDistance, characters));
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

        public bool CheckIfCharacterOnTile(ITile tile)
        {
            return GetAllCharacters().Exists(player => player.XPosition == tile.XPosition && player.YPosition == tile.YPosition);
        }

        public char[,] GetMapAroundCharacter(Character character)
        {
            var characters = ((IEnumerable<Character>)_players).Concat(_creatures).ToList();
            return _map.GetMapAroundCharacter(character, _viewDistance, characters);
        }

        private List<Character> GetAllCharacters()
        {
            List<Character> characters = _players.Cast<Character>().ToList();
            return characters;
        }

        public void LoadArea(int playerX, int playerY, int viewDistance)
        {
            _map.LoadArea(playerX, playerY, viewDistance);
        }

        public void UpdateAI()
        {
            movesList = new List<Character>();
            foreach (Character monster in _creatures)
            {
                if (monster is SmartMonster smartMonster)
                {
                    if (smartMonster.Brain != null)
                    {
                        UpdateSmartMonster(smartMonster);
                    }
                }
            }
        }

        private void UpdateSmartMonster(SmartMonster smartMonster)
        {
            smartMonster.Update();
            movesList.Add(smartMonster);
        }

        public Character GetAI(string id)
        {
            foreach (Character ai in _creatures)
            {
                if (ai.Id == id)
                {
                    return ai;
                }
            }
            return null;
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