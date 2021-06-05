using Creature.Creature;
using Creature.Creature.NeuralNetworking;
using Items;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UserInterface;

namespace WorldGeneration
{
    [ExcludeFromCodeCoverage]
    public class WorldService : IWorldService
    {
        private IScreenHandler _screenHandler;
        private World _world;

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

        public void AddCreatureToWorld(Character character)
        {
            _world.AddCreatureToWorld(character);
        }

        public void DisplayWorld()
        {
            _world.UpdateMap();
        }

        public void DeleteMap()
        {
            _world.DeleteMap();
        }

        public void GenerateWorld(int seed)
        {
            _world = new World(seed, 6, new MapFactory(), _screenHandler);
        }

        public Player getCurrentPlayer()
        {
            return _world.CurrentPlayer;
        }

        public World GetWorld()
        {
            return _world;
        }

        public char[,] GetMapAroundCharacter(Character character)
        {
            return _world.GetMapAroundCharacter(character);
        }

        public List<Player> GetPlayers()
        {
            return _world._players;
        }

        public List<Character> GetMonsters()
        {
            return _world._creatures;
        }

        public void UpdateBrains(Genome genome)
        {
            foreach (Character monster in _world._creatures)
            {
                if (monster is SmartMonster smartMonster)
                {
                    smartMonster.brain = genome;
                }
            }
        }

        public List<Character> getCreatureMoves()
        {
            return _world.movesList;
        }

        public Player GetPlayer(string userId)
        {
            return _world?.GetPlayer(userId);
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

        public void DisplayStats()
        {
            Player player = getCurrentPlayer();
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
        }
    }
}