using System;

namespace WorldGeneration
{
    public class WorldService : IWorldService
    {
        private World _world;

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

        public Player GetCurrentPlayer()
        {
            return _world.CurrentPlayer;
        }

        public Player GetPlayer(string userId)
        {
            return _world?.GetPlayer(userId);
        }

<<<<<<< HEAD
        public IList<Item> GetItemsOnCurrentTile()
        {
            return _world.GetCurrentTile().ItemsOnTile;
        }

        public IList<Item> GetItemsOnCurrentTile(Player player)
        {
            return _world.GetTileForPlayer(player).ItemsOnTile;
        }

=======
>>>>>>> parent of a6db6d52 (Merge pull request #153 in VDFZEH/asd-project-game from sub-task/VDFZEH-498-coderen-uitvoeren-commando-pickup-item to feature/VDFZEH-483-coderen-commando-use-n)
        public string SearchCurrentTile()
        {
            var itemsOnCurrentTile = _world.GetCurrentTile().ItemsOnTile;

            string result = "The following items are on the current tile:" + Environment.NewLine;
            int index = 1;
            foreach (var item in itemsOnCurrentTile)
            {
                result += $"{index}. {item.ItemName}{Environment.NewLine}";
                index += 1;
            }
            return result;
        }
    }
}