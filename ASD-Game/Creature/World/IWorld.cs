using System.Collections.Generic;
using ASD_Game.Creature.Creature;
using ASD_Game.Creature.Pathfinder;

namespace ASD_Game.Creature.World
{
    public interface IWorld
    {
        public List<ICreature> Creatures { get; }
        public List<ICreature> Players { get; }

        public List<List<Node>> Nodes { get; }
        public int Size { get; }

        public void GenerateWorldNodes();
        public void SpawnPlayer(ICreature player);
        public void SpawnCreature(ICreature creature);
        public void Render();
    }
}
