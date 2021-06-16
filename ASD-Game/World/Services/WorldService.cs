using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using ASD_Game.Items;
using ASD_Game.Items.Services;
using ASD_Game.Messages;
using ASD_Game.UserInterface;
using ASD_Game.World.Models.Characters;
using ASD_Game.World.Models.Characters.Algorithms.NeuralNetworking;
using ASD_Game.World.Models.Interfaces;
using ASD_Game.Session.GameConfiguration;

namespace ASD_Game.World.Services
{
    public class WorldService : IWorldService
    {
        public IItemService ItemService { get; }
        private readonly IGameConfigurationHandler _gameConfigHandler;
        private readonly IScreenHandler _screenHandler;
        private readonly IMessageService _messageService;
        private IWorld _world;
        public List<Character> CreatureMoves { get; set; }
        private const int VIEWDISTANCE = 6;
        private bool _logicSet = false;
        private IEnemySpawner _enemySpawner;

        private bool gameEnded = false;

        public WorldService(IScreenHandler screenHandler, IItemService itemService, IMessageService messageService, IGameConfigurationHandler gameConfigurationHandler)
        {
            _gameConfigHandler = gameConfigurationHandler;
            _screenHandler = screenHandler;
            ItemService = itemService;
            _enemySpawner = new EnemySpawner();
            _messageService = messageService;
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
            _world.AddMonsterToWorld(character);
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
            _world = new World(seed, VIEWDISTANCE, new MapFactory(), _screenHandler, ItemService, _enemySpawner, _gameConfigHandler);
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
            return _world.Monsters;
        }

        public List<Character> GetAllCharacters()
        {
            return _world.GetAllCharacters();
        }

        public Character GetCharacterInClosestRangeToCurrentCharacter(Character currentCharacter, int distance)
        {
            var monsters = GetMonsters();
            var players = GetAllPlayers();

            foreach (var player in players)
            {
                if (currentCharacter != null)
                {
                    if (Vector2.Distance(new Vector2(currentCharacter.XPosition, currentCharacter.YPosition), new Vector2(player.XPosition, player.YPosition)) <= distance
                        && player.Id != currentCharacter.Id)
                    {
                        if (player.Health > 0)
                        {
                            return player;
                        }
                    }
                }
            }

            foreach (var monster in monsters)
            {
                if (currentCharacter != null)
                {
                    if (Vector2.Distance(new Vector2(currentCharacter.XPosition, currentCharacter.YPosition), new Vector2(monster.XPosition, monster.YPosition)) <= distance
                        && monster.Id != currentCharacter.Id)
                    {
                        return monster;
                    }
                }
            }

            return null;
        }

        public Monster GetMonsterInClosestRangeToCurrentCharacter(Character currentCharacter, int distance)
        {
            var monsters = GetMonsters();
            
            foreach (var monster in monsters)
            {
                if (currentCharacter != null)
                {
                    if (Vector2.Distance(new Vector2(currentCharacter.XPosition, currentCharacter.YPosition), new Vector2(monster.XPosition, monster.YPosition)) <= distance
                        && monster.Id != currentCharacter.Id)
                    {
                        return monster;
                    }
                }
            }
            return null;
        }

        public Player GetPlayerInClosestRangeToCurrentCharacter(Character currentCharacter, int distance)
        {
            var players = GetAllPlayers();

            foreach (var player in players)
            {
                if (currentCharacter != null)
                {
                    if (Vector2.Distance(new Vector2(currentCharacter.XPosition, currentCharacter.YPosition), new Vector2(player.XPosition, player.YPosition)) <= distance
                        && player.Id != currentCharacter.Id)
                    {
                        if (player.Health > 0)
                        {
                            return player;
                        }
                    }
                }
            }

            return null;
        }

        public Character GetCharacter(string id)
        {
            var characters = _world.GetAllCharacters();

            return characters.Find(character => character.Id == id);
        }

        public void UpdateBrains(Genome genome)
        {
            if (_world != null)
            {
                foreach (Character monster in _world.Monsters)
                {
                    if (monster is SmartMonster smartMonster)
                    {
                        smartMonster.Brain = genome;
                    }
                    else if (monster is Monster monst)
                    {
                        if (!monst.MonsterStateMachine.WasStarted())
                        {
                            monst.MonsterData.WorldService = this;
                            monst.MonsterStateMachine.StartStateMachine();
                        }
                    }
                }
            }
        }

        public void SetAILogic()
        {
            List<SmartMonster> setup = new();
            foreach (Character monster in _world.Monsters)
            {
                if (monster is SmartMonster smartMonster)
                {
                    smartMonster._dataGatheringService = new DataGatheringService(this);
                    setup.Add((SmartMonster)monster);
                }
                else if (monster is Monster monst)
                {
                    monst.MonsterData.WorldService = this;
                    monst.MonsterStateMachine.StartStateMachine();
                }
            }
            SetAIActions(setup);
        }

        private void SetAIActions(List<SmartMonster> smartMonsters)
        {
            foreach (SmartMonster monster in smartMonsters)
            {
                SmartMonster smartMonster = (SmartMonster)_world.GetAI(monster.Id);
                smartMonster.Smartactions = new SmartCreatureActions(smartMonster);
            }
        }

        public List<Character> GetCreatureMoves(string type)
        {
            if (_world != null)
            {
                if (type == "Attack")
                {
                    _world.AttackList.Clear();
                    _world.UpdateAI();
                    return _world.AttackList;
                }
                else
                {
                    _world.MovesList.Clear();
                    _world.UpdateAI();
                    return _world.MovesList;
                }
            }
            return null;
        }

        public List<Player> GetAllPlayers()
        {
            if (_world != null)
            {
                return _world.Players;
            }
            return null;
        }

        public bool IsDead(Player player)
        {
            return player.Health <= 0;
        }

        public void LoadArea(int playerX, int playerY, int viewDistance)
        {
            _world.LoadArea(playerX, playerY, viewDistance);
        }

        public IList<Item> GetItemsOnCurrentTileWithPlayerId(string playerId)
        {
            var player = GetPlayer(playerId);
            return _world.GetTileForPlayer(player).ItemsOnTile;
        }

        public string SearchCurrentTile()
        {
            var itemsOnCurrentTile = GetItemsOnCurrentTile();

            if (itemsOnCurrentTile.Count == 0)
            {
                return "There are no items on the current tile!";
            }

            var result = new StringBuilder();
            result.Append("The following items are on the current tile:");

            var index = 1;
            foreach (var item in itemsOnCurrentTile)
            {
                result.Append($"{Environment.NewLine}{index}. {item.ItemName}");
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
                GetAllPlayers().Count(player => player.Health > 0),
                GetAllPlayers().Count,
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

        public void CheckLastManStanding()
        {
            if (gameEnded || GetAllPlayers().Count == 1)
            {
                return;
            }

            int livingPlayers = 0;
            Player livingPlayer = null;

            foreach (var player in GetAllPlayers().Where(player => player.Health > 0))
            {
                livingPlayers++;
                livingPlayer = player;
            }

            if (livingPlayers == 1)
            {
                _messageService.AddMessage(livingPlayer.Name + " is the last player in the game and won! Congratulations!");
                gameEnded = true;
            }
        }

        public Character GetCharacterOnTile(int x, int y)
        {
            var characters = GetAllCharacters();
            return characters.Find(character => character.XPosition == x && character.YPosition == y);
        }
    }
}