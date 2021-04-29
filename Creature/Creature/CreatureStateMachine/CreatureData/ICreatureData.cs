using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creature.Creature.StateMachine.CreatureData
{
    interface ICreatureData
    {
        bool IsAlive { get; set; }
        System.Numerics.Vector2 Position { get; set; }
        int VisionRange { get; set; }
        int Damage { get; set; }
        int Health { get; set; }

    }
}
