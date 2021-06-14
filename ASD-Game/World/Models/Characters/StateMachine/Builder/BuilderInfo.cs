using System.Collections.Generic;
using ASD_Game.Creature.Creature.StateMachine.CustomRuleSet;
using ASD_Game.World.Models.Characters.StateMachine.State;
using WorldGeneration.StateMachine.CustomRuleSet;
using WorldGeneration.StateMachine.Event;

namespace ASD_Game.World.Models.Characters.StateMachine.Builder
{
    public class BuilderInfo
    {
        private string _action;
        private List<CharacterState> _initialSates;
        private CharacterEvent.Event _event;
        private List<RuleSet> _ruleSet;
        private CharacterState _targetState;

        public string Action
        {
            get => _action;
            set => _action = value;
        }

        public List<CharacterState> InitialStates
        {
            get => _initialSates;
            set => _initialSates = value;
        }

        public CharacterEvent.Event Event
        {
            get => _event;
            set => _event = value;
        }

        public List<RuleSet> RuleSets
        {
            get => _ruleSet;
            set => _ruleSet = value;
        }

        public CharacterState TargetState
        {
            get => _targetState;
            set => _targetState = value;
        }

        public BuilderInfo()
        {
            _initialSates = new List<CharacterState>();
            _ruleSet = new List<RuleSet>();
        }
    }
}
