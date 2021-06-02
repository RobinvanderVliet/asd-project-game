using System.Collections.Generic;
using Items;

namespace WorldGeneration
{
    public interface IWorldService
    {
        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition);
        public void AddPlayerToWorld(Player player, bool isCurrentPlayer);
        public void DisplayWorld();
        public void DeleteMap();
        public void GenerateWorld(int seed);
        public IList<Item> GetItemsOnCurrentTile();
        public IList<Item> GetItemsOnCurrentTile(Player player);
        public void DropItemOnCurrentTile(Player player, Item Item);
        public string SearchCurrentTile();
        public Player GetCurrentPlayer();
        public Player GetPlayer(string userId);
    }
}