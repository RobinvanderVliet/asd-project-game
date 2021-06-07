using WorldGeneration.Models.Interfaces;
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

        public List<Player> GetAllPlayers();

        public void PlayerDied(Player currentPlayer);

        public bool IsDead(Player player);
        /// <summary>
        /// Returns items on title for the player of this instance.
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Returns items on tile for the specified player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
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
    }
}