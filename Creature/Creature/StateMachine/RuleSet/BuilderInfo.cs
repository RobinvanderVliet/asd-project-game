using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creature.Creature.StateMachine.CustomRuleSet
{
    public class BuilderInfo
    {
        private string _action;
        private List<CreatureState> _initialSates;
        private CreatureEvent.Event _event;
        private RuleSet _ruleSet;
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

        public RuleSet RuleSet
        {
            get => _ruleSet;
            set => _ruleSet = value;
        }

        public CreatureState TargetState
        {
            get => _targetState;
            set => _targetState = value;
        }
    }
}
