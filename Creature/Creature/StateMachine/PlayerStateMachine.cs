using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;

namespace Creature.Creature.StateMachine
{
    class PlayerStateMachine : ICreatureStateMachine
    {
        private RuleSet _ruleset;
        private PassiveStateMachine<CreatureState, CreatureEvent> _passiveStateMachine;
        private PlayerData _playerData;

        public PlayerStateMachine(PlayerData playerData, RuleSet ruleSet)
        {
            _playerData = playerData;
            _ruleset = ruleSet;
        }

        public ICreatureData CreatureData {
            get => _playerData;
            set => _playerData = (PlayerData)value;
        }

        public void FireEvent(CreatureEvent creatureEvent, object argument)
        {
            if (creatureEvent.GetType() == typeof(CreatureEvent))
            {
                _passiveStateMachine.Fire(creatureEvent, argument);
            }
        }

        public void FireEvent(CreatureEvent creatureEvent)
        {
            if (creatureEvent.GetType() == typeof(CreatureEvent))
            {
                _passiveStateMachine.Fire(creatureEvent);
            }
        }

        public void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CreatureState, CreatureEvent>();

            // TODO: implement statemachine for player

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
        }
    }
}
