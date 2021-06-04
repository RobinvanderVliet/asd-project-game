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
        /// <summary>
        /// Returns items on title for the player of this instance.
        /// </summary>
        /// <returns></returns>
        public IList<Item> GetItemsOnCurrentTile();
        /// <summary>
        /// Returns items on tile for the specified player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public IList<Item> GetItemsOnCurrentTile(Player player);
        public string SearchCurrentTile();
        public Player GetCurrentPlayer();
        public Player GetPlayer(string userId);
        public void DisplayStats();
    }
}