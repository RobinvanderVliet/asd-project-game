using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;
using System.Collections.Generic;

namespace Creature.Creature.StateMachine.Builder
{
    public class BuilderInfo
    {
        private string _action;
        private List<CreatureState> _initialSates;
        private CreatureEvent.Event _event;
        private List<RuleSet> _ruleSet;
        private CreatureState _targetState;

        public string Action
        {
            get => _action;
            set => _action = value;
        }

        public List<CreatureState> InitialStates
        {
            get => _initialSates;
            set => _initialSates = value;
        }

        public CreatureEvent.Event Event
        {
            get => _event;
            set => _event = value;
        }

        public List<RuleSet> RuleSets
        {
            get => _ruleSet;
            set => _ruleSet = value;
        }

        public CreatureState TargetState
        {
            get => _targetState;
            set => _targetState = value;
        }

        public BuilderInfo()
        {
            _initialSates = new List<CreatureState>();
            _ruleSet = new List<RuleSet>();
        }
    }
}
