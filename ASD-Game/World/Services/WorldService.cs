using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using UserInterface;

namespace WorldGeneration
{
    [ExcludeFromCodeCoverage]
    public class WorldService : IWorldService
    {
        private IScreenHandler _screenHandler;
        private World _world;

        public WorldService(IScreenHandler screenHandler)
        {
            _screenHandler = screenHandler;
        }

        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition)
        {
            _world.UpdateCharacterPosition(userId, newXPosition, newYPosition);
        }

        public void AddPlayerToWorld(Player player, bool isCurrentPlayer)
        {
            _world.AddPlayerToWorld(player, isCurrentPlayer);
        }

        public void AddCreatureToWorld(Character character)
        {
            _world.AddCreatureToWorld(character);
        }

        public void DisplayWorld()
        {
            _world.UpdateMap();
        }

        public void DeleteMap()
        {
            _world.DeleteMap();
        }

        public void GenerateWorld(int seed)
        {
            _world = new World(seed, 6, new MapFactory(), _screenHandler);
        }

        public Player getCurrentPlayer()
        {
            return _world.CurrentPlayer;
        }

        public World GetWorld()
        {
            return _world;
        }

        public char[,] GetMapAroundCharacter(Character character)
        {
            return _world.GetMapAroundCharacter(character);
        }

        public List<Player> GetPlayers()
        {
            return _world._players;
        }

        public List<Character> GetMonsters()
        {
            return _world._creatures;
        }

        public List<Character> getCreatureMoves()
        {
            return _world.movesList;
        }
    }
}