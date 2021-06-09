using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using World.Models.Characters.StateMachine.Data;
using WorldGeneration.StateMachine.Event;
using WorldGeneration.StateMachine.State;

namespace WorldGeneration.StateMachine
{
    public abstract class DefaultStateMachine : ICharacterStateMachine
    {
        public PassiveStateMachine<CharacterState, CharacterEvent.Event> _passiveStateMachine;
        protected ICharacterData _characterData;

        [ExcludeFromCodeCoverage]
        public ICharacterData CharacterData
        {
            get => _characterData;
            set => _characterData = value;
        }

        protected Timer _timer;

        public DefaultStateMachine(ICharacterData characterData)
        {
            _characterData = characterData;
        }

        [ExcludeFromCodeCoverage]
        public virtual void StartStateMachine()
        {
            _passiveStateMachine.Start();
        }

        [ExcludeFromCodeCoverage]
        public void StopStateMachine()
        {
            _timer.Dispose();
            _passiveStateMachine.Stop();
        }

        protected void Update()
        {
            _timer = new Timer((e) =>
            {
                FireEvent(CharacterEvent.Event.DO);
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
        }
        
        protected void KillLoop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
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
            builder.In(state).On(CharacterEvent.Event.DO).Execute(state.Do);
        }
    }
}