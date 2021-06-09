using ASD_Game.ActionHandling.DTO;
using ASD_Game.World.Models.Characters;
using System.Collections.Generic;

namespace ASD_Game.World
{
    public interface IWorld
    {
        public List<Player> Players { get; set; }
        public List<Monster> Creatures { get; set; }

        void UpdateCharacterPosition(string id, int newXPosition, int newYPosition);

        void AddPlayerToWorld(Player player, bool isCurrentPlayer = false);

        void AddCreatureToWorld(Monster monster);

        void UpdateMap();

        char[,] GetMapAroundCharacter(Character character);

        void DeleteMap();
        void AddItemToWorld(ItemSpawnDTO itemSpawnDTO);
    }
}