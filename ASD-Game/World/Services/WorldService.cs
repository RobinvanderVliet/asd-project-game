using Items;
using WorldGeneration.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UserInterface;
using World.Models.Characters.Algorithms.NeuralNetworking;
using World.Models.Characters;

namespace WorldGeneration
{
    [ExcludeFromCodeCoverage]
    public class WorldService : IWorldService
    {
        private IScreenHandler _screenHandler;
        private World _world;

        public List<Character> _creatureMoves { get; set; }

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

        public Player GetCurrentPlayer()
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

        public List<Character> GetMonsters()
        {
            return _world._creatures;
        }

        public void UpdateBrains(Genome genome)
        {
            if (_world != null)
            {
                foreach (Character monster in _world._creatures)
                {
                    if (monster is SmartMonster smartMonster)
                    {
                        smartMonster.Brain = genome;
                    }
                }
            }
        }

        public List<Character> GetCreatureMoves()
        {
            if (_world != null)
            {
                _world.UpdateAI();
                return _world.movesList;
            }
            return null;
        }

        public List<Player> GetAllPlayers()
        {
            return _world.Players;
        }

        public bool IsDead(Player player)
        {
            return player.Health <= 0;
        }

        public IList<Item> GetItemsOnCurrentTile()
        {
            return _world.GetCurrentTile().ItemsOnTile;
        }

        public IList<Item> GetItemsOnCurrentTile(Player player)
        {
            return _world.GetTileForPlayer(player).ItemsOnTile;
        }

        public IList<Item> GetItemsOnCurrentTileWithPlayerId(string playerId)
        {
            var player = GetPlayer(playerId);
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

        public Character GetAI(string id)
        {
            return _world.GetAI(id);
        }

        public ITile GetTile(int x, int y)
        {
            return _world.GetLoadedTileByXAndY(x, y);
        }

        public bool CheckIfCharacterOnTile(ITile tile)
        {
            return _world.CheckIfCharacterOnTile(tile);
        }

        public void LoadArea(int playerX, int playerY, int viewDistance)
        {
            _world.LoadArea(playerX, playerY, viewDistance);
        }

        public void DisplayStats()
        {
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
        }
    }
}