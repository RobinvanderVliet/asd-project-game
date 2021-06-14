using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.Items.Services;
using ASD_Game.UserInterface;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World
{
    public class World : IWorld
    {
        private IMap _map;
        public Player CurrentPlayer { get; set; }
        public List<Player> Players { get; set; }
        public List<Monster> Creatures { get; set; }
        public List<Character> MovesList { get; set; }
        public List<ItemSpawnDTO> Items { get; set; }
        private readonly int _viewDistance;
        private readonly IScreenHandler _screenHandler;
        private static readonly char _separator = Path.DirectorySeparatorChar;

        public World(int seed, int viewDistance, IMapFactory mapFactory, IScreenHandler screenHandler, IItemService itemService)
        {
            // Players = new();
            // _creatures = new();
            // var currentDirectory = Directory.GetCurrentDirectory();
            //
            // Players = new();
            // _viewDistance = viewDistance;
            // _screenHandler = screenHandler;
            // DeleteMap();
            Items = new();
            Players = new ();
            Creatures = new ();
            _map = mapFactory.GenerateMap(itemService, Items, seed);
            _viewDistance = viewDistance;
            _screenHandler = screenHandler;
        }
        
        public Player GetPlayer(string id)
        {
            return Players.Find(x => x.Id == id);
        }

        public void UpdateCharacterPosition(string userId, int newXPosition, int newYPosition)
        {
            if (CurrentPlayer != null && CurrentPlayer.Id == userId)
            {
                CurrentPlayer.XPosition = newXPosition;
                CurrentPlayer.YPosition = newYPosition;
            }
            else if (GetPlayer(userId) != null)
            {
                var player = GetPlayer(userId);
                player.XPosition = newXPosition;
                player.YPosition = newYPosition;
            }
            var creature = Creatures.FirstOrDefault(x => x.Id == userId);
            if (GetAI(userId) != null)
            {
                creature.XPosition = newXPosition;
                creature.YPosition = newYPosition;
            }
        }

        public void AddPlayerToWorld(Player player, bool isCurrentPlayer = false)
        {
            if (isCurrentPlayer)
            {
                CurrentPlayer = player;
            }
            Players.Add(player);
            UpdateMap();
        }

        public void AddCreatureToWorld(Monster character)
        {
            Creatures.Add(character);
        }

        public void UpdateMap()
        {
            if (CurrentPlayer != null && Players != null && Creatures != null)
            {
                _screenHandler.UpdateWorld(_map.GetCharArrayMapAroundCharacter(CurrentPlayer, _viewDistance, GetAllCharacters()));
            }
        }

        public void DeleteMap()
        {
            _map.DeleteMap();
        }

        public void AddItemToWorld(ItemSpawnDTO itemSpawnDto)
        {
            Items.Add(itemSpawnDto);
            UpdateMap();
        }
        public ITile GetLoadedTileByXAndY(int x, int y)
        {
            return _map.GetLoadedTileByXAndY(x, y);
        }

        public bool CheckIfCharacterOnTile(ITile tile)
        {
            return GetAllCharacters().Exists(player => player.XPosition == tile.XPosition && player.YPosition == tile.YPosition);
        }

        public char[,] GetMapAroundCharacter(Character character)
        {
            return _map.GetCharArrayMapAroundCharacter(character, _viewDistance, GetAllCharacters());
        }

        public List<Character> GetAllCharacters()
        {
            List<Character> characters = Players.Cast<Character>().ToList();
            characters.AddRange(Creatures);
            return characters;
        }

        public void LoadArea(int playerX, int playerY, int viewDistance)
        {
            _map.LoadArea(playerX, playerY, viewDistance);
        }
         
        public void UpdateAI()
        {
            MovesList = new List<Character>();
            foreach (Character monster in Creatures)
            {
                if (monster is SmartMonster smartMonster)
                {
                    if (smartMonster.Brain != null)
                    {
                        UpdateSmartMonster(smartMonster);
                    }
                }
            }
        }

        private void UpdateSmartMonster(SmartMonster smartMonster)
        {
            smartMonster.Update();
            MovesList.Add(smartMonster);
        }

        public Character GetAI(string id)
        {
            return Creatures.Find(x => x.Id == id);
        }

        public ITile GetCurrentTile()
        {
            return _map.GetLoadedTileByXAndY(CurrentPlayer.XPosition, CurrentPlayer.YPosition);
        }

        public ITile GetTileForPlayer(Player player)
        {
            return _map.GetLoadedTileByXAndY(player.XPosition, player.YPosition);
        }

        public List<Player> GetAllPlayers()
        {
            return Players;
        }
    }
}