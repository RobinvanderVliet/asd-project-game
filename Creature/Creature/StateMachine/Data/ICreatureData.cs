using Creature.Creature.StateMachine.State;
using Creature.World;

namespace Creature.Creature.StateMachine.Data
{
    public interface ICreatureData
    {
        bool IsAlive { get; set; }
        System.Numerics.Vector2 Position { get; set; }
        int VisionRange { get; set; }
        int Damage { get; set; }
        int Health { get; set; }
        IWorld World { get; set;  }

    }
}
