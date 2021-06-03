using System.Numerics;

namespace WorldGeneration.StateMachine.Data
{
    public interface ICharacterData
    {
        bool IsAlive { get; }
        Vector2 Position { get; set; }
        int VisionRange { get; set; }
        int Damage { get; set; }
        double Health { get; set; }
        // TODO:: Fix integration with world
        // IWorld World { get; set;  }

    }
}
