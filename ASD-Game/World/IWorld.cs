using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration
{
    public interface IWorld
    {
        public List<Character> _creatures { get; set; }

        void UpdateCharacterPosition(string id, int newXPosition, int newYPosition);

        void AddPlayerToWorld(Player player, bool isCurrentPlayer = false);

        void AddCreatureToWorld(Character player);

        void UpdateMap();

        char[,] GetMapAroundCharacter(Character character);

        void DeleteMap();
    }
}