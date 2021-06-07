using System.Collections.Generic;
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
        public Player CurrentPlayer;
        private IMap _map;
        private List<Models.Characters.Creature> _creatures;
        private List<Player> _players;
        public List<ItemSpawnDTO> Items;
        private readonly int _viewDistance;
        private readonly IScreenHandler _screenHandler;
        private static readonly char _separator = Path.DirectorySeparatorChar;
        private IItemService _itemService;
        

        public World(int seed, int viewDistance, IMapFactory mapFactory, IScreenHandler screenHandler, IItemService itemService)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            Items = new();
            _players = new ();
            _creatures = new ();
            _itemService = itemService;
            _map = mapFactory.GenerateMap(itemService, Items, seed);
            _viewDistance = viewDistance;
            _screenHandler = screenHandler;
            itemService.GetSpawnHandler().SetItemSpawnDtos(Items);
        }
        
        public Player GetPlayer(string id)
        {
            return _players.Find(x => x.Id == id);
        }

        public void UpdateCharacterPosition(string id, int newXPosition, int newYPosition)
        {
            var player = GetPlayer(id);
            if (player != null)
            {
                player.XPosition = newXPosition;
                player.YPosition = newYPosition;
            }
            var creature = _creatures.FirstOrDefault(x => x.Id == id);
            if (creature != null)
            {
                creature.XPosition = newXPosition;
                creature.YPosition = newYPosition;
            }
            UpdateMap();
        }

        public void AddPlayerToWorld(Player player, bool isCurrentPlayer = false)
        {
            if (isCurrentPlayer)
            {
                CurrentPlayer = player;
            }
            _players.Add(player);
            UpdateMap();
        }
        
        public void AddCreatureToWorld(Models.Characters.Creature creature)
        {
            _creatures.Add(creature);
            UpdateMap();
        }

        public void UpdateMap()
        {
            if (CurrentPlayer != null && _players != null && _creatures != null)
            {
                var characters = ((IEnumerable<Character>)_players).Concat(_creatures).ToList();
                var a = CurrentPlayer;
                var b = _viewDistance;
                var c = _map;
                var mapArray = _map.GetCharArrayMapAroundCharacter(CurrentPlayer, _viewDistance, characters);
                _screenHandler.UpdateWorld(mapArray);
                //_map.DisplayMap(CurrentPlayer, _viewDistance, characters); in case the UI breaks, this will do
            }
        }

        public char[,] GetMapAroundCharacter(Character character)
        {
            var characters = ((IEnumerable<Character>)_players).Concat(_creatures).ToList();
            return _map.GetCharArrayMapAroundCharacter(character, _viewDistance, characters);
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

        private List<Character> GetAllCharacters()
        {
            List<Character> characters = _players.Cast<Character>().ToList();
            return characters;
        }

        public ITile GetCurrentTile()
        {
            return _map.GetLoadedTileByXAndY(CurrentPlayer.XPosition, CurrentPlayer.YPosition);
        }

        public ITile GetTileForPlayer(Player player)
        {
            return _map.GetLoadedTileByXAndY(player.XPosition, player.YPosition);
        }
        
        public void LoadArea(int playerX, int playerY, int viewDistance)
        {
            _map.LoadArea(playerX, playerY, viewDistance);
        }

        public List<Player> GetAllPlayers()
        {
            return _players;
        }
    }
}
     
