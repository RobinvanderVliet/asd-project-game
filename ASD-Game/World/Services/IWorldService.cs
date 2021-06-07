using System.Collections.Generic;
using ASD_project.ActionHandling.DTO;
using ASD_project.Items;
using ASD_project.World.Models.Characters;
using ASD_project.World.Models.Interfaces;

namespace ASD_project.World.Services
{
    public interface IWorldService
    {
        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition);
        public void AddPlayerToWorld(Player player, bool isCurrentPlayer);
        public void DisplayWorld();
        public void DeleteMap();
        public void GenerateWorld(int seed);
        public Player getCurrentPlayer();
        public List<ItemSpawnDTO> getAllItems();
        public ITile GetTile(int x, int y);
        public void AddItemToWorld(ItemSpawnDTO itemSpawnDto);
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