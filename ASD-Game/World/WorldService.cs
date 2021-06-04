using WorldGeneration.Models.Interfaces;
using System;
using System.Collections.Generic;
using Items;
using UserInterface;

namespace WorldGeneration
{
    public class WorldService : IWorldService
    {
        private World _world;
        private bool displayingStats;

        private IScreenHandler _screenHandler;

        public WorldService(IScreenHandler screenHandler)
        {
            _screenHandler = screenHandler;
            displayingStats = false;
        }

        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition)
        {
            _world.UpdateCharacterPosition(userId, newXPosition, newYPosition);
        }

        public void AddPlayerToWorld(Player player, bool isCurrentPlayer)
        {
            _world.AddPlayerToWorld(player, isCurrentPlayer);
        }

        public void DisplayWorld()
        {
            _world.DisplayWorld();
        }

        public void DeleteMap()
        {
            _world.DeleteMap();
        }

        public void GenerateWorld(int seed)
        {
            _world = new World(seed, 6, _screenHandler);
        }
        
        public Player GetCurrentPlayer()
        {
            return _world.CurrentPlayer;
        }

        public IList<Item> GetItemsOnCurrentTile()
        {
            return _world.GetCurrentTile().ItemsOnTile;
        }

        public IList<Item> GetItemsOnCurrentTile(Player player)
        {
            return _world.GetTileForPlayer(player).ItemsOnTile;
        }

        public string SearchCurrentTile()
        {
            var itemsOnCurrentTile = GetItemsOnCurrentTile();

            string result = "The following items are on the current tile:" + Environment.NewLine;
            int index = 1;
            foreach (var item in itemsOnCurrentTile)
            {
                result += $"{index}. {item.ItemName}{Environment.NewLine}";
                index += 1;
            }
            return result;
        }

        public Player GetPlayer(string userId)
        {
            return _world.GetPlayer(userId);
        }

        public ITile GetTile(int x, int y)
        {
            return _world.GetLoadedTileByXAndY(x, y);
        }
        
        public bool CheckIfPlayerOnTile(ITile tile)
        {
            return _world.CheckIfPlayerOnTile(tile);
        }

        public void LoadArea(int playerX, int playerY, int viewDistance)
        {
            _world.LoadArea(playerX, playerY, viewDistance);
        }

         public void DisplayStats()
         {
            if (!displayingStats)
            {
                displayingStats = true;
                Player player = GetCurrentPlayer();
                _screenHandler.SetStatValues(
                    player.Name,
                    0,
                    player.Health,
                    player.Stamina,
                    player.GetArmorPoints(),
                    player.RadiationLevel,
                    player.Inventory.Helmet?.ItemName ?? "Empty",
                    player.Inventory.Armor?.ItemName ?? "Empty",
                    player.Inventory.Weapon?.ItemName ?? "Empty",
                    player.Inventory.GetConsumableAtIndex(0)?.ItemName ?? "Empty",
                    player.Inventory.GetConsumableAtIndex(1)?.ItemName ?? "Empty",
                    player.Inventory.GetConsumableAtIndex(2)?.ItemName ?? "Empty");
                displayingStats = false;
            }
        }

        public List<Player> GetPlayers()
        {
            return _world.GetAllPlayers();
        }
    }
}