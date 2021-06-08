using System.Collections.Generic;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.Items;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World.Services
{
    public interface IWorldService
    {
        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition);
        public void AddPlayerToWorld(Player player, bool isCurrentPlayer);
        public void DisplayWorld();
        public void DeleteMap();
        public void GenerateWorld(int seed);
        public List<Player> GetAllPlayers();
        public bool IsDead(Player player);
        public Player getCurrentPlayer();
        public List<ItemSpawnDTO> getAllItems();
        public ITile GetTile(int x, int y);
        public void AddItemToWorld(ItemSpawnDTO itemSpawnDto);
        /// Returns items on title for the player of this instance.
        public IList<Item> GetItemsOnCurrentTile();
        /// Returns items on tile for the specified player.
        public IList<Item> GetItemsOnCurrentTile(Player player);
        public Player GetCurrentPlayer();
        public Player GetPlayer(string id);
        public List<Player> GetPlayers();
        public bool CheckIfCharacterOnTile(ITile tile);
        public void LoadArea(int playerX, int playerY, int viewDistance);
        public string SearchCurrentTile();
        public void DisplayStats();
    }
}