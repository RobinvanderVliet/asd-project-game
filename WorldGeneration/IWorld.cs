using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldGeneration
{
    public interface IWorld
    {
        void UpdateCharacterPosition(string id, int newXPosition, int newYPosition);
        void AddPlayerToWorld(Player player, bool isCurrentPlayer = false);
        void AddCreatureToWorld(Creature player);
        void updateWorld();
        void DeleteMap();
    }
}
