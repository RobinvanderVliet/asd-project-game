using System.Collections.Generic;
using System.Linq;
using ASD_project.ActionHandling.DTO;
using ASD_project.Items.Services;
using ASD_project.UserInterface;
using ASD_project.World.Models.Characters;

namespace ASD_project.World
{
    public class World : IWorld
    {
        public Player CurrentPlayer;
        private IMap _map;
        private List<Models.Characters.Creature> _creatures;
        private List<Player> _players;
        public List<ItemSpawnDTO> Items;
        private readonly int _viewDistance;
        private IScreenHandler _screenHandler;
        private IItemService _itemService;
        

        public World(int seed, int viewDistance, IMapFactory mapFactory, IScreenHandler screenHandler, IItemService itemService)
        {
            Items = new();
            _players = new ();
            _creatures = new ();
            _itemService = itemService;
            _map = mapFactory.GenerateMap(itemService, Items, seed);
            _viewDistance = viewDistance;
            _screenHandler = screenHandler;
            itemService.GetSpawnHandler().SetItemSpawnDtOs(Items);
        }

        public void UpdateCharacterPosition(string id, int newXPosition, int newYPosition)
        {
            var player = _players.FirstOrDefault(x => x.Id == id);
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
                //_screenHandler.UpdateWorld(_map.GetMapAroundCharacter(CurrentPlayer, _viewDistance, characters));
                _map.DisplayMap(CurrentPlayer, _viewDistance, characters);
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
    }
}
     
