using Creature.World;
using System.Collections.Generic;
using System.Numerics;

namespace Creature.Creature.StateMachine.Data
{
    public interface ICreatureData
    {
        bool IsAlive { get; }
        Vector2 Position { get; set; }
        int VisionRange { get; set; }
        int Damage { get; set; }
        double Health { get; set; }
        IWorld World { get; set;  }

        List<Dictionary<string, string>> RuleSet { get; }

    }
}
