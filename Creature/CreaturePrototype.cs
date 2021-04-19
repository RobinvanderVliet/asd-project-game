using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creature
{
    class CreaturePrototype
    {
        static void Main(string[] args)
        {
            ICreature creature = new Monster();
            creature.FireEvent(Monster.Event.SPOTTED_PLAYER);
        }
    }
}
