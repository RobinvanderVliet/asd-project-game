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

            CreatureState idleState = new IdleState(CreatureData);
            CreatureState wanderState = new WanderState(CreatureData);
            CreatureState inventoryState = new InventoryState(CreatureData);
            CreatureState useConsumableState = new UseConsumableState(CreatureData);
            CreatureState followCreatureState = new FollowCreatureState(CreatureData);
            CreatureState attackState = new AttackState(CreatureData);
            CreatureState fleeFromCreatureState = new FleeFromCreatureState(CreatureData);
           

            // Idle
            builder.In(wanderState).On(CreatureEvent.Event.IDLE).Goto(idleState).Execute<ICreatureData>(new WanderState(CreatureData).Do);
            // Wandering
            builder.In(followCreatureState).On(CreatureEvent.Event.LOST_CREATURE).Goto(wanderState).Execute<ICreatureData>(new WanderState(CreatureData).Do);
            builder.In(fleeFromCreatureState).On(CreatureEvent.Event.LOST_CREATURE).Goto(wanderState).Execute<ICreatureData>(new WanderState(CreatureData).Do);
            // Manage inventory
            builder.In(wanderState).On(CreatureEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICreatureData>(new InventoryState(CreatureData).Do);
            builder.In(fleeFromCreatureState).On(CreatureEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICreatureData>(new InventoryState(CreatureData).Do);
            builder.In(followCreatureState).On(CreatureEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICreatureData>(new InventoryState(CreatureData).Do);
            // Use Consumable
            builder.In(fleeFromCreatureState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);
            builder.In(followCreatureState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);
            // Follow creature
            builder.In(wanderState).On(CreatureEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            builder.In(attackState).On(CreatureEvent.Event.CREATURE_OUT_OF_RANGE).Goto(followCreatureState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            // Attack creature
            builder.In(followCreatureState).On(CreatureEvent.Event.CREATURE_IN_RANGE).Goto(attackState).Execute<ICreatureData>(new AttackState(CreatureData).Do);
            builder.In(attackState).On(CreatureEvent.Event.CREATURE_IN_RANGE).Execute<ICreatureData>(new AttackState(CreatureData).Do);
            // Flee From creature
            builder.In(attackState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(fleeFromCreatureState).Execute<ICreatureData>(new FleeFromCreatureState(CreatureData).Do);

            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
        }
    }
}