using Creature.Creature.NeuralNetworking;
using Items;
using System.Collections.Generic;

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

        public IList<Item> GetItemsOnCurrentTile();

        List<Character> getCreatureMoves();

        List<Player> GetPlayers();

        List<Character> GetMonsters();

        public void UpdateBrains(Genome genome);

        public char[,] GetMapAroundCharacter(Character character);

        public IList<Item> GetItemsOnCurrentTile(Player player);

        public string SearchCurrentTile();

        public Player getCurrentPlayer();

        public Player GetPlayer(string userId);

        public void DisplayStats();
    }
}