using System.Collections.Generic;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.Items;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World.Services
{
    public interface IWorldService
    {
        public void AddPlayerToWorld(Player player, bool isCurrentPlayer);
        public void DisplayWorld();
        public void GenerateWorld(int seed);
        public bool IsDead(Player player);
        public ITile GetTile(int x, int y);
        public IList<Item> GetItemsOnCurrentTile(Player player);
        public Player GetCurrentPlayer();
        public Player GetPlayer(string id);
        public List<Player> GetAllPlayers();
        public bool CheckIfCharacterOnTile(ITile tile);
        public void LoadArea(int playerX, int playerY, int viewDistance);
        public string SearchCurrentTile();
        public void DisplayStats();
    }
}