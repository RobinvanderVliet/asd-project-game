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
        private ICreatureData _agentData;

        public AgentStateMachine(ICreatureData agentData)
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

            CreatureState idleState = new IdleState();
            CreatureState wanderState = new WanderState();
            CreatureState inventoryState = new InventoryState();
            CreatureState useConsumableState = new UseConsumableState();
            CreatureState followCreatureState = new FollowCreatureState();
            CreatureState attackState = new AttackState();
            CreatureState fleeFromCreatureState = new FleeFromCreatureState();
            
            // Idle
            builder.In(wanderState).On(CreatureEvent.Event.IDLE).Goto(idleState).Execute<ICreatureData>(wanderState.SetCreatureData); //IF not using CreatureDate then remove execute.
            // Wandering
            builder.In(followCreatureState).On(CreatureEvent.Event.LOST_CREATURE).Goto(wanderState).Execute<ICreatureData>(wanderState.SetCreatureData);
            builder.In(fleeFromCreatureState).On(CreatureEvent.Event.LOST_CREATURE).Goto(wanderState).Execute<ICreatureData>(wanderState.SetCreatureData);
            // Manage inventory
            builder.In(wanderState).On(CreatureEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICreatureData>(inventoryState.SetCreatureData);
            builder.In(fleeFromCreatureState).On(CreatureEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICreatureData>(inventoryState.SetCreatureData);
            builder.In(followCreatureState).On(CreatureEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICreatureData>(inventoryState.SetCreatureData);
            // Use Consumable
            builder.In(fleeFromCreatureState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICreatureData>(useConsumableState.SetCreatureData);
            builder.In(followCreatureState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICreatureData>(useConsumableState.SetCreatureData);
            builder.In(wanderState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICreatureData>(useConsumableState.SetCreatureData);
            builder.In(fleeFromCreatureState).On(CreatureEvent.Event.OUT_OF_STAMINA).Goto(useConsumableState).Execute<ICreatureData>(useConsumableState.SetCreatureData);
            builder.In(followCreatureState).On(CreatureEvent.Event.OUT_OF_STAMINA).Goto(useConsumableState).Execute<ICreatureData>(useConsumableState.SetCreatureData);
            builder.In(wanderState).On(CreatureEvent.Event.OUT_OF_STAMINA).Goto(useConsumableState).Execute<ICreatureData>(useConsumableState.SetCreatureData);
            // Follow creature
            builder.In(wanderState).On(CreatureEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState).Execute<ICreatureData>(followCreatureState.SetCreatureData);
            builder.In(attackState).On(CreatureEvent.Event.CREATURE_OUT_OF_RANGE).Goto(followCreatureState).Execute<ICreatureData>(followCreatureState.SetCreatureData);
            // Attack creature
            builder.In(followCreatureState).On(CreatureEvent.Event.CREATURE_IN_RANGE).Goto(attackState).Execute<ICreatureData>(attackState.SetCreatureData);
            builder.In(attackState).On(CreatureEvent.Event.CREATURE_IN_RANGE).Execute<ICreatureData>(attackState.SetCreatureData);
            builder.In(attackState).On(CreatureEvent.Event.OUT_OF_STAMINA).Execute<ICreatureData>(attackState.SetCreatureData);
            // Flee From creature
            builder.In(attackState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(fleeFromCreatureState).Execute<ICreatureData>(fleeFromCreatureState.SetCreatureData); //TODO add SetTarget();

            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
        }
    }
}