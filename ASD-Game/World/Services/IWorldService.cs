using System.Collections.Generic;
using System.Numerics;

namespace WorldGeneration
{
    public interface IWorldService
    {
        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition);

        public void AddPlayerToWorld(Player player, bool isCurrentPlayer);

        public void AddCreatureToWorld(Character character);

        public void DisplayWorld();

        public void DeleteMap();

        public void GenerateWorld(int seed);

        public Player getCurrentPlayer();

        List<Character> getCreatureMoves();

        List<Player> GetPlayers();

        List<Character> GetMonsters();

        public char[,] GetMapAroundCharacter(Character character);
    }
}