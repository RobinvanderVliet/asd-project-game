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
            _world.DisplayWorld();
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
            if (_screenHandler.Screen is GameScreen)
            {
                Player player = getCurrentPlayer();
                GameScreen screen = _screenHandler.Screen as GameScreen;
                screen.SetStatValues(
                    player.Name,
                    0,
                    player.Health,
                    player.Stamina,
                    player.Inventory.Armor.ArmorProtectionPoints + player.Inventory.Helmet.ArmorProtectionPoints,
                    player.RadiationLevel,
                    player.Inventory.Helmet.ItemName,
                    player.Inventory.Armor.ItemName,
                    player.Inventory.Weapon.ItemName,
                    player.Inventory.ConsumableItemList[0].ItemName,
                    player.Inventory.ConsumableItemList[1].ItemName,
                    player.Inventory.ConsumableItemList[2].ItemName
                    );
            }
        }
    }
}