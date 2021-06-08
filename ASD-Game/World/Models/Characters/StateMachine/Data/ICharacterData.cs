using System.Numerics;

namespace ASD_Game.World.Models.Characters.StateMachine.Data
{
    public interface ICharacterData
    {
        bool IsAlive { get; }
        Vector2 Position { get; set; }
        int VisionRange { get; set; }
        int Damage { get; set; }
        double Health { get; set; }
        IWorld World { get; set; }

    }
}
