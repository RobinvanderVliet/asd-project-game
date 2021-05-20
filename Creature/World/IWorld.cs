using Creature.Pathfinder;
using System.Collections.Generic;

namespace Creature.World
{
    public interface IWorld
    {
        public List<ICreature> Creatures { get; }
        public List<ICreature> Players { get; }

        public List<List<Node>> Nodes { get; }
        public int Size { get; }

        public void GenerateWorldNodes();
        public void SpawnPlayer(ICreature player);
        public void SpawnAgent(ICreature agent);
        public void SpawnCreature(ICreature creature);
        public void Render();
    }
}
