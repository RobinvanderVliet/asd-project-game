using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;

namespace WorldGeneration.StateMachine
{
    public class PlayerStateMachine : DefaultStateMachine
    {
        public PlayerStateMachine(ICharacterData characterData, RuleSet ruleSet) : base(characterData, ruleSet)
        {
        }
        public ICharacterData CharacterData {
            get => _characterData;
            set => _characterData = (PlayerData)value;
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
