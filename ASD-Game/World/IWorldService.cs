using WorldGeneration.Models.Interfaces;
 using System.Collections.Generic;
using Items;

namespace WorldGeneration
{
    public interface IWorldService
    {
        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition);
        public void AddPlayerToWorld(Player player, bool isCurrentPlayer);
        public void AddExistingPlayerToWorld(Player player, bool isCurrentPlayer);
        public void DisplayWorld();
        public void DeleteMap();
        public void GenerateWorld(int seed);
        public IList<Item> GetItemsOnCurrentTile(Player player);
        public IList<Item> GetItemsOnCurrentTile();
        public Player GetCurrentPlayer();
        public Player GetPlayer(string id);
        public List<Player> GetPlayers();
        public ITile GetTile(int x, int y);
        public bool CheckIfCharacterOnTile(ITile tile);
        public void LoadArea(int playerX, int playerY, int viewDistance);
        public string SearchCurrentTile();
        public void DisplayStats();
        public Player getCurrentPlayer();

    }
}