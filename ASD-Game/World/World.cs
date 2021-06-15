using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASD_Game.ActionHandling.DTO;
using ASD_Game.Items.Services;
using ASD_Game.UserInterface;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using ASD_Game.World.Models.Interfaces;

namespace ASD_Game.World
{
    public class World : IWorld
    {
        private IMap _map;
        public Player CurrentPlayer { get; set; }
        public List<Player> Players { get; set; }
        public List<Monster> DeadCreatures { get; set; } = new List<Monster>();
        public List<Character> MovesList { get; set; } = new List<Character>();
        public List<Character> AttackList { get; set; } = new List<Character>();
        public List<Monster> Monsters { get; set; }
        public List<ItemSpawnDTO> Items { get; set; }
        private readonly int _viewDistance;
        private readonly IScreenHandler _screenHandler;

        public World(int seed, int viewDistance, IMapFactory mapFactory, IScreenHandler screenHandler, IItemService itemService, IEnemySpawner enemySpawner)
        {
            Items = new();
            Players = new();
            Monsters = new();
            _map = mapFactory.GenerateMap(itemService, enemySpawner, Items, Monsters, seed);
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
            var creature = Monsters.FirstOrDefault(x => x.Id == userId);
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

        public void AddMonsterToWorld(Monster character)
        {
            Monsters.Add(character);
        }

        public void UpdateMap()
        {
            if (CurrentPlayer != null && Players != null && Monsters != null)
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
            characters.AddRange(Monsters);
            return characters;
        }

        public void LoadArea(int playerX, int playerY, int viewDistance)
        {
            _map.LoadArea(playerX, playerY, viewDistance);
        }

        public void UpdateAI()
        {
            deleteDeadMonsters();
            foreach (Character monster in Monsters)
            {
                if (monster.Health <= 0)
                {
                    DeadCreatures.Add((Monster)monster);
                }
                if (monster is SmartMonster smartMonster)
                {
                    if (smartMonster.Brain == null)
                    {
                        smartMonster.Brain = new Genome(14, 8);
                        UpdateSmartMonster(smartMonster);
                    }
                    else
                    {
                        UpdateSmartMonster(smartMonster);
                    }
                }
            }
        }

        public void deleteDeadMonsters()
        {
            if (DeadCreatures != null)
            {
                foreach (Monster monster in DeadCreatures)
                {
                    string montid = monster.Id;
                    Monster x = (Monster)GetAI(montid);
                    Monsters.Remove(x);
                }
            }
        }

        private void UpdateSmartMonster(SmartMonster smartMonster)
        {
            smartMonster.Update();
            if (smartMonster.MoveType == "Move")
            {
                MovesList.Add(smartMonster);
            }
            else
            {
                AttackList.Add(smartMonster);
            }
        }

        public Character GetAI(string id)
        {
            return Monsters.Find(x => x.Id == id);
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