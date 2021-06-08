﻿using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ASD_Game.Creature.World;

namespace ASD_Game.Creature.Creature.StateMachine.Data
{
    [ExcludeFromCodeCoverage]
    public class MonsterData : ICreatureData
    {
        private Vector2 _position;
        private double _health;
        private int _damage;
        private int _visionRange;
        private IWorld _world;
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

        public bool IsFollowing
        {
            get => _following;
            set => _following = value;
        }

        public MonsterData(Vector2 position, double health, int damage, int visionRange, IWorld world, bool following)
        {
            _position = position;
            _health = health;
            _damage = damage;
            _visionRange = visionRange;
            _world = world;
            _following = following;
        }
    }
}