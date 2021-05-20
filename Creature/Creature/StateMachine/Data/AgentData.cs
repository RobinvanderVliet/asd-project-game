﻿using System.Numerics;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.World;

namespace Creature.Creature.StateMachine.Data
{
    public class AgentData : ICreatureData
    {
        private Vector2 _position;
        private double _health;
        private int _damage;
        private int _visionRange;
        private IWorld _world;
        private bool _following;
        private RuleSet _ruleSet;

        public RuleSet RuleSet { get; set; }
        public bool IsAlive
        {
            get => _health > 0;
        }

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

        public bool IsFollowing
        {
            get => _following;
            set => _following = value;
        }
        
        public AgentData(Vector2 position, double health, int damage, int visionRange, IWorld world, bool following,
            RuleSet agentRuleSet)
        {
            _position = position;
            _health = health;
            _damage = damage;
            _visionRange = visionRange;
            _world = world;
            _following = following;
            _ruleSet = agentRuleSet;
        }
    }
}