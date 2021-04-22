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
        List<ICreature> creatures { get; }
        List<ICreature> players { get; }
        int Size { get; }

        public void GenerateWorldNodes();
        public void SpawnPlayer(ICreature player);
        public void SpawnCreature(ICreature creature);
        public void Render();
    }
}
