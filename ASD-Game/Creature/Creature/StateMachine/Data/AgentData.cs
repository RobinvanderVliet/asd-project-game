using System;
using System.Collections.Generic;
using System.Numerics;

namespace Creature.Creature.StateMachine.Data
{
    public class AgentData : ICreatureData
    {
        private Vector2 _position;
        private double _health;
        private double _stamina;
        private int _visionRange;
        private List<ValueTuple<string, string>> _ruleSet;
        private bool _following;
        
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
        
        public int VisionRange
        {
            get => _visionRange;
            set => _visionRange = value;
        }

        public List<ValueTuple<string, string>> RuleSet
        {
            get => _ruleSet;
        }

        public bool IsFollowing
        {
            get => _following;
            set => _following = value;
        }
        
        public AgentData(Vector2 position, double health, double stamina, int visionRange, List<ValueTuple<string, string>> agentRuleSet, bool following)
        {
            _position = position;
            _health = health;
            _stamina = stamina;
            _visionRange = visionRange;
            _following = following;
            _ruleSet = agentRuleSet;
        }
    }
}