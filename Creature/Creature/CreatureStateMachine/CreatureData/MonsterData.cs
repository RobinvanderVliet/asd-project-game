using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Creature.Creature.StateMachine.CreatureData
{
    class MonsterData : ICreatureData
    {
        private bool _alive;
        private Vector2 _position;
        private int _health;
        private int _damage;
        private int _visionRange;
        private bool _following;

        public bool IsAlive
        {
            get => _alive;
            set => _alive = value;
        }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }
        
        public int Health
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

        public bool IsFollowing
        {
            get => _following;
            set => _following = value;
        }
    }
}
