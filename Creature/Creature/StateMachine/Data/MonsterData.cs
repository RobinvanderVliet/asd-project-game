using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using System.Numerics;

namespace Creature.Creature.StateMachine.Data
{
    [ExcludeFromCodeCoverage]
    public class MonsterData : ICreatureData
    {
        private Vector2 _position;
        private double _health;
        private int _damage;
        private int _visionRange;

        private List<Dictionary<string, string>> _ruleSet;
        private bool _following;

        public bool IsAlive => _health > 0;

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

        public double Stamina { get; set; }

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
        

        public List<Dictionary<string, string>> RuleSet
        {
            get => _ruleSet;
        }

        public bool IsFollowing
        {
            get => _following;
            set => _following = value;
        }

        public MonsterData(Vector2 position, double health, int damage, int visionRange, List<Dictionary<string, string>> ruleSet, bool following)
        {
            _position = position;
            _health = health;
            _damage = damage;
            _visionRange = visionRange;
            _ruleSet = ruleSet;
            _following = following;
        }
    }
}