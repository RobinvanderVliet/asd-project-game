using System;
using System.Collections.Generic;
using System.Text;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.Items;
using ASD_Game.Items.Services;
using ASD_Game.UserInterface;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World.Services
{
    public class WorldService : IWorldService
    {
        private readonly IItemService _itemService;
        private readonly IScreenHandler _screenHandler;
        private IWorld _world;
        public List<Character> CreatureMoves { get; set; }
        private const int VIEWDISTANCE = 6;
        
        public WorldService(IScreenHandler screenHandler, IItemService itemService)
        {
            _screenHandler = screenHandler;
            _itemService = itemService;
        }

        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition)
        {
            _world.UpdateCharacterPosition(userId, newXPosition, newYPosition);
        }

        public void AddPlayerToWorld(Player player, bool isCurrentPlayer)
        {
            _world.AddPlayerToWorld(player, isCurrentPlayer);
        }

        public void SetWorld(IWorld world)
        {
            _world = world;
        }

        public void AddCreatureToWorld(Monster character)
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
            _world = new World(seed, VIEWDISTANCE, new MapFactory(), _screenHandler, _itemService);
        }

        public Player GetCurrentPlayer()
        {
            return _world.CurrentPlayer;
        }

        public IWorld GetWorld()
        {
            return _world;
        }

        public char[,] GetMapAroundCharacter(Character character)
        {
            return _world.GetMapAroundCharacter(character);
        }

        public List<Monster> GetMonsters()
        {
            return _world.Creatures;
        }

        public List<Character> GetAllCharacters()
        {
            return _world.GetAllCharacters();
        }

        public void UpdateBrains(Genome genome)
        {
            if (_world != null)
            {
                foreach (Character monster in _world.Creatures)
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
                return _world.MovesList;
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
        
        public void LoadArea(int playerX, int playerY, int viewDistance)
        {
            _world.LoadArea(playerX, playerY, viewDistance);
        }

        public string SearchCurrentTile()
        {
            var itemsOnCurrentTile = GetItemsOnCurrentTile();
            StringBuilder result = new StringBuilder();

            result.Append("The following items are on the current tile:" + Environment.NewLine);

            var index = 1;
            foreach (var item in itemsOnCurrentTile)
            {
                result.Append($"{index}. {item.ItemName}{Environment.NewLine}");
                index += 1;
            }

            return result.ToString();
        }
        
        public Player GetPlayer(string id)
        {
            return _world.GetPlayer(id);
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
        public IList<Item> GetItemsOnCurrentTile()
        {
            return _world.GetCurrentTile().ItemsOnTile;
        }


        public IList<Item> GetItemsOnCurrentTile(Player player)
        {
            return _world.GetTileForPlayer(player).ItemsOnTile;
        }

        public int GetViewDistance()
        {
            return VIEWDISTANCE;
        }
    }
}