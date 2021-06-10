using Appccelerate.StateMachine.Machine;
using ASD_Game.World.Models.Characters.StateMachine.CustomRuleSet;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;
using WorldGeneration.StateMachine.Data;
using WorldGeneration.StateMachine.Event;

namespace ASD_Game.World.Models.Characters.StateMachine
{
    public class PlayerStateMachine : DefaultStateMachine
    {
        public PlayerStateMachine(ICharacterData characterData, RuleSet ruleSet) : base(characterData, ruleSet)
        {
        }

        public ICharacterData CharacterData
        {
            get => _characterData;
            set => _characterData = (PlayerData)value;
        }

        public void FireEvent(CharacterEvent.Event creatureEvent, object argument)
        {
            _passiveStateMachine.Fire(creatureEvent, argument);
        }

        public void FireEvent(CharacterEvent.Event creatureEvent)
        {
            _passiveStateMachine.Fire(creatureEvent);
        }

        public void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CharacterState, CharacterEvent.Event>();

            // TODO: implement statemachine for player

            //_passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            //_passiveStateMachine.Start();
        }
    }
}