using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Creature.World
{
    interface IWorld
    {
        List<ICreature> creatures { get; }
        List<IPlayer> players { get; }
        int Size { get; }
        public void SpawnCreature(ICreature creature);
        public void SpawnPlayer(IPlayer player);
        public void Render();
    }
}
