using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using ASD_Game.World.Models.Characters.StateMachine.CustomRuleSet;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;
using WorldGeneration.StateMachine;
using WorldGeneration.StateMachine.Event;

namespace ASD_Game.World.Models.Characters.StateMachine
{
    public abstract class DefaultStateMachine : ICharacterStateMachine
    {
        protected RuleSet _ruleset;
        public PassiveStateMachine<CharacterState, CharacterEvent.Event> _passiveStateMachine;
        protected ICharacterData _characterData;

        [ExcludeFromCodeCoverage]
        public ICharacterData CharacterData
        {
            get => _characterData;
            set => _characterData = value;
        }

        protected Timer _timer;

        public DefaultStateMachine(ICharacterData characterData, RuleSet ruleset)
        {
            _characterData = characterData;
            _ruleset = ruleset;
        }

        [ExcludeFromCodeCoverage]
        public virtual void StartStateMachine()
        {
            _passiveStateMachine.Start();
        }

        protected void Update()
        {
            _timer = new Timer((e) =>
            {
                FireEvent(CharacterEvent.Event.DO);
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
        }

        public void FireEvent(CharacterEvent.Event creatureEvent, object argument)
        {
            _passiveStateMachine.Fire(creatureEvent, argument);
        }

        public void FireEvent(CharacterEvent.Event creatureEvent)
        {
            _passiveStateMachine.Fire(creatureEvent);
        }

        protected void DefineDefaultBehaviour(
            ref StateMachineDefinitionBuilder<CharacterState, CharacterEvent.Event> builder, ref CharacterState state)
        {
            builder.In(state).ExecuteOnEntry(state.Entry);
            builder.In(state).ExecuteOnExit(state.Exit);
            builder.In(state).On(CharacterEvent.Event.DO).Execute(state.Do);
        }
    }
}