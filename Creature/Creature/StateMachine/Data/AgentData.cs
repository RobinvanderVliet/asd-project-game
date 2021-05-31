using System.Collections.Generic;
using System.Numerics;
using Creature.World;

namespace Creature.Creature.StateMachine.Data
{
    public class AgentData : ICreatureData
    {
        private Vector2 _position;
        private double _health;
        private double _stamina;
        private int _damage;
        private int _visionRange;
        private bool _following;
        private List<Dictionary<string, string>> _ruleSet;

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

        public double Stamina
        {
            get => _stamina;
            set => _stamina = value;
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

        public List<Dictionary<string, string>> RuleSet
        {
            get => _ruleSet;
        }

        public bool IsFollowing
        {
            get => _following;
            set => _following = value;
        }
        
        public AgentData(Vector2 position, double health, double stamina, int damage, int visionRange, List<Dictionary<string, string>> agentRuleSet, bool following)
        {
            _position = position;
            _health = health;
            _stamina = stamina;
            _damage = damage;
            _visionRange = visionRange;
            _following = following;
            _ruleSet = agentRuleSet;
        }
    }
}