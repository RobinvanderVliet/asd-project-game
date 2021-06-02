using System;
using System.Collections.Generic;
using System.Numerics;
using Creature.World;

namespace Creature.Creature.StateMachine.Data
{
    public interface ICreatureData
    {
        bool IsAlive { get; }
        Vector2 Position { get; set; }
        int VisionRange { get; set; }
        int Damage { get; set; }
        double Health { get; set; }
        List<ValueTuple<string, string>> RuleSet { get; }
        IWorld World { get; set; } //TODO check if used, Otherwise delete ALSO check the classes that implement this.
    }
}