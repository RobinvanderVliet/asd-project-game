using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;

namespace Creature.Creature.StateMachine
{
    public class AgentStateMachine : ICreatureStateMachine
    {
        private PassiveStateMachine<CreatureState, CreatureEvent.Event> _passiveStateMachine;
        private AgentData _agentData;

        public AgentStateMachine(AgentData agentData)
        {
            _agentData = agentData;
        }

        public ICreatureData CreatureData
        {
            get => _agentData;
            set => _agentData = (AgentData)value;
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

            CreatureState followPlayerState = new FollowCreatureState(CreatureData);
            CreatureState wanderState = new WanderState(CreatureData);
            CreatureState useConsumableState = new UseConsumableState(CreatureData);
            CreatureState attackPlayerState = new AttackState(CreatureData);
            CreatureState fleeState = new FleeState(CreatureData);

            // Wandering
            builder.In(followPlayerState).On(CreatureEvent.Event.LOST_PLAYER).Goto(wanderState);

            // Follow player
            builder.In(wanderState).On(CreatureEvent.Event.SPOTTED_PLAYER).Goto(followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            builder.In(followPlayerState).On(CreatureEvent.Event.SPOTTED_PLAYER).Goto(followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            builder.In(useConsumableState).On(CreatureEvent.Event.REGAINED_HEALTH_PLAYER_OUT_OF_RANGE).Goto(followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            builder.In(attackPlayerState).On(CreatureEvent.Event.PLAYER_OUT_OF_RANGE).Goto(followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);

            // Attack player
            builder.In(followPlayerState).On(CreatureEvent.Event.PLAYER_IN_RANGE).Goto(attackPlayerState).Execute<ICreatureData>(new AttackState(CreatureData).Do);
            builder.In(attackPlayerState).On(CreatureEvent.Event.PLAYER_IN_RANGE).Execute<ICreatureData>(new AttackState(CreatureData).Do);
            builder.In(useConsumableState).On(CreatureEvent.Event.REGAINED_HEALTH_PLAYER_IN_RANGE).Goto(attackPlayerState).Execute<ICreatureData>(new AttackState(CreatureData).Do);

            // Use potion
            builder.In(fleeState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);
            builder.In(followPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);

            //Flee 
            builder.In(attackPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(fleeState).Execute<ICreatureData>(new FleeState(CreatureData).Do);
            
            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
        }
    }
}