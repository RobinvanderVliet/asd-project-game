using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using System.Numerics;

namespace Creature.Creature.StateMachine.Data
{
    [ExcludeFromCodeCoverage]
    public class PlayerData : ICreatureData
    {
        private Vector2 _position;
        private double _health;
        private int _damage;
        private int _visionRange;
        private List<ValueTuple<string, string>> _ruleSet;

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

        public List<ValueTuple<string, string>> RuleSet
        {
            get => _ruleSet;
        }

        public PlayerData(Vector2 position, double health, int damage, int visionRange, List<ValueTuple<string, string>> ruleSet)
        {
            _position = position;
            _health = health;
            _damage = damage;
            _visionRange = visionRange;
            _ruleSet = ruleSet;
        }
    }
}
