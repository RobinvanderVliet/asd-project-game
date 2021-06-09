using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ASD_Game.World;
using ASD_Game.World.Models.Characters.StateMachine.Data;

namespace WorldGeneration.StateMachine.Data
{
    [ExcludeFromCodeCoverage]
    public class PlayerData : ICharacterData
    {
        private Vector2 _position;
        private double _health;
        private int _damage;
        private int _visionRange;
        // TODO:: Fix integration with world
        // private IWorld _world;

        public bool IsAlive { get => _health > 0; }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public double Health
        {
            get => _health;
            set => _health = value;
        }

        public IWorld World { get; set; }

        public int Damage
        {
            get => _damage;
            set => _damage = value;
        }

        public int VisionRange
        {
            get => _visionRange;
            set => _visionRange = value;
        }

        // public IWorld World
        // {
        //     get => _world;
        //     set => _world = value;
        // }

        public PlayerData(Vector2 position, double health, int damage, int visionRange/**, IWorld world**/)
        {
            _position = position;
            _health = health;
            _damage = damage;
            _visionRange = visionRange;
            // _world = world;
        }
    }
}
