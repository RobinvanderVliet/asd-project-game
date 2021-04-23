using Creature.Pathfinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
        public void SpawnCreature(ICreature creature);
        public void Render();
    }
}
