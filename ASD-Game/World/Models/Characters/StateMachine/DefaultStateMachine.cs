using System;
using System.Threading;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;

namespace WorldGeneration.StateMachine
{
    public abstract class DefaultStateMachine : ICharacterStateMachine
    {
        protected RuleSet _ruleset;
        protected PassiveStateMachine<CreatureState, CreatureEvent.Event> _passiveStateMachine;
        protected ICharacterData _characterData;
        
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
        
        public virtual void StartStateMachine()
        {
            _passiveStateMachine.Start();
        }
        
        protected void Update()
        {
            _timer = new Timer((e) =>
            {
                FireEvent(CreatureEvent.Event.DO);
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
        }

        protected void KillLoop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void FireEvent(CreatureEvent.Event creatureEvent, object argument)
        {
            _passiveStateMachine.Fire(creatureEvent, argument);
        }

        public void FireEvent(CreatureEvent.Event creatureEvent)
        {
            _passiveStateMachine.Fire(creatureEvent);
        }

        protected void DefineDefaultBehaviour(
            ref StateMachineDefinitionBuilder<CreatureState, CreatureEvent.Event> builder, ref CreatureState state)
        {
            builder.In(state).ExecuteOnEntry(state.Entry);
            builder.In(state).ExecuteOnExit(state.Exit);
            builder.In(state).On(CreatureEvent.Event.DO).Execute(state.Do);
        }
    }
}