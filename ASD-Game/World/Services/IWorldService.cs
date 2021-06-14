using System.Collections.Generic;
using ASD_Game.Items;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World.Services
{
    public interface IWorldService
    {
        List<Character> CreatureMoves { get; set; }

        public void SetWorld(IWorld world);

        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition);

        public void AddPlayerToWorld(Player player, bool isCurrentPlayer);

        public void AddCreatureToWorld(Monster character);

        public void DisplayWorld();

        public void DeleteMap();

        public void GenerateWorld(int seed);

        public List<Player> GetAllPlayers();

        public bool IsDead(Player player);

        public IList<Item> GetItemsOnCurrentTile(Player player);

        public IList<Item> GetItemsOnCurrentTile();

        List<Character> GetCreatureMoves();

        List<Monster> GetMonsters();
        public List<Character> GetAllCharacters();

        public void UpdateBrains(Genome genome);

        public char[,] GetMapAroundCharacter(Character character);

        public Player GetCurrentPlayer();

        public Player GetPlayer(string id);

        public ITile GetTile(int x, int y);

        public bool CheckIfCharacterOnTile(ITile tile);

        public void LoadArea(int playerX, int playerY, int viewDistance);

        public string SearchCurrentTile();

        public void DisplayStats();

        public Character GetAI(string id);

        public int GetViewDistance();
    }
}