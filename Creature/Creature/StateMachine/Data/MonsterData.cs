using Creature.World;
using System.Collections.Generic;
using System.Numerics;

namespace Creature.Creature.StateMachine.Data
{
    public class MonsterData : ICreatureData
    {
        private Vector2 _position;
        private double _health;
        private int _damage;
        private int _visionRange;
        private IWorld _world;
        private Dictionary<string, string> _ruleSet;
        private bool _following;

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

        public IWorld World
        {
            get => _world;
            set => _world = value;
        }

        public Dictionary<string, string> RuleSet
        {
            get => _ruleSet;
        }

        public bool IsFollowing
        {
            get => _following;
            set => _following = value;
        }

        public MonsterData(Vector2 position, double health, int damage, int visionRange, IWorld world, Dictionary<string, string> ruleSet, bool following)
        {
            _position = position;
            _health = health;
            _damage = damage;
            _visionRange = visionRange;
            _world = world;
            _ruleSet = ruleSet;
            _following = following;
        }
    }
}