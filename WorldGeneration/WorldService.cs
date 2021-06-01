using UserInterface;

namespace WorldGeneration
{
    public class WorldService : IWorldService
    {
        private World _world;
        private IScreenHandler _screenHandler;

        public WorldService(IScreenHandler screenHandler)
        {
            _screenHandler = screenHandler;
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
            //_world.DisplayWorld();
        }
        
        public void DeleteMap()
        {
            _world.deleteMap();
        }

        public void GenerateWorld(int seed)
        {
            _world = new World(seed, 6);
        }

        public Player getCurrentPlayer()
        {
            return _world.CurrentPlayer;
        }
        //TODO Player score
        public void UpdatePlayerStatScreen()
        {
                Player player = getCurrentPlayer();
                _screenHandler.SetStatValues(
                    player.Name,
                    0,
                    player.Health,
                    player.Stamina,
                    player.Inventory.Armor?.ArmorProtectionPoints ?? 0 + player.Inventory.Helmet?.ArmorProtectionPoints ?? 0,
                    player.RadiationLevel,
                    player.Inventory.Helmet?.ItemName ?? "Empty",
                    player.Inventory.Armor?.ItemName ?? "Empty",
                    player.Inventory.Weapon?.ItemName ?? "Empty",
                    player.Inventory.ConsumableItemList.Count >= 1 ? player.Inventory.ConsumableItemList[0].ItemName: "Empty",
                    player.Inventory.ConsumableItemList.Count >= 2 ? player.Inventory.ConsumableItemList[1].ItemName: "Empty",
                    player.Inventory.ConsumableItemList.Count == 3 ? player.Inventory.ConsumableItemList[2].ItemName: "Empty"
                    );        
        }
    }
}