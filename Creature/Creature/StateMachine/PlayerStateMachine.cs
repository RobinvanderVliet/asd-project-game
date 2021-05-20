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
        private PassiveStateMachine<CreatureState, CreatureEvent.Event> _passiveStateMachine;
        private PlayerData _playerData;

        public PlayerStateMachine(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public ICreatureData CreatureData {
            get => _playerData;
            set => _playerData = (PlayerData)value;
        }

        public void FireEvent(CreatureEvent.Event creatureEvent, object argument)
        {
            _passiveStateMachine.Fire(creatureEvent, argument);
        }

        public void FireEvent(CreatureEvent.Event creatureEvent)
        {
            _passiveStateMachine.Fire(creatureEvent);
        }

        public void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CreatureState, CreatureEvent.Event>();

            // TODO: implement statemachine for player

            //_passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            //_passiveStateMachine.Start();
        }
    }
}
