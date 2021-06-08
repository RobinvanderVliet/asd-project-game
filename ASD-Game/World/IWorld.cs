using System.Collections.Generic;

namespace WorldGeneration
{
    public interface IWorld
    {
        public List<Player> Players { get; set; }
        public List<Character> _creatures { get; set; }

        void UpdateCharacterPosition(string id, int newXPosition, int newYPosition);

        void AddPlayerToWorld(Player player, bool isCurrentPlayer = false);

        void AddCreatureToWorld(Character player);

        void UpdateMap();

        char[,] GetMapAroundCharacter(Character character);

        void DeleteMap();
    }
}