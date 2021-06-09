using System.Threading;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.State;
using World.Models.Characters.StateMachine.Builder;
using World.Models.Characters.StateMachine.Data;
using WorldGeneration.StateMachine;
using WorldGeneration.StateMachine.Event;
using WorldGeneration.StateMachine.State;

namespace World.Models.Characters.StateMachine
{
    public class AgentStateMachine : DefaultStateMachine
    {
        private Timer _timer;

        public AgentStateMachine(ICharacterData characterData) : base(characterData)
        {
        }

        public ICharacterData CharacterData
        {
            get => _characterData;
            set => _characterData = (AgentData) value;
        }

        public override void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CharacterState, CharacterEvent.Event>();
            var ruleSetFactory = new RuleSetFactory();
            var rulesetList = ruleSetFactory.GetRuleSetListFromSettingsList(CharacterData.RuleSet);
            var builderConfigurator = new BuilderConfigurator(rulesetList, CharacterData, this);
            var builderInfoList = builderConfigurator.GetBuilderInfoList();

            CharacterData.BuilderConfigurator = builderConfigurator;

            CharacterState idleState = new IdleState(CharacterData);
            CharacterState inventoryState = new InventoryState(CharacterData);
            CharacterState fleeFromCharacterState = new FleeFromCreatureState(CharacterData);
            CharacterState followCreatureState = new FollowCreatureState(CharacterData);
            CharacterState wanderState = new WanderState(CharacterData);
            CharacterState useConsumableState = new UseConsumableState(CharacterData);
            CharacterState attackState = new AttackState(CharacterData);
                
            DefineDefaultBehaviour(ref builder, ref followCreatureState);
            DefineDefaultBehaviour(ref builder, ref wanderState);
            DefineDefaultBehaviour(ref builder, ref useConsumableState);
            DefineDefaultBehaviour(ref builder, ref attackState);
            DefineDefaultBehaviour(ref builder, ref idleState);
            DefineDefaultBehaviour(ref builder, ref inventoryState);
            DefineDefaultBehaviour(ref builder, ref fleeFromCharacterState);

            // Idle
            builder.In(wanderState).On(CharacterEvent.Event.IDLE).Goto(idleState).Execute<ICharacterData>(wanderState.SetTargetData); //IF not using a Target then remove execute.
            
            // Wandering
            builder.In(followCreatureState).On(CharacterEvent.Event.LOST_CREATURE).Goto(wanderState).Execute<ICharacterData>(wanderState.SetTargetData);
            builder.In(fleeFromCharacterState).On(CharacterEvent.Event.LOST_CREATURE).Goto(wanderState).Execute<ICharacterData>(wanderState.SetTargetData);
            builder.In(idleState).On(CharacterEvent.Event.WANDERING).Goto(wanderState).Execute<ICharacterData>(wanderState.SetTargetData);
            
            // Manage inventory
            builder.In(wanderState).On(CharacterEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICharacterData>(inventoryState.SetTargetData);
            builder.In(fleeFromCharacterState).On(CharacterEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICharacterData>(inventoryState.SetTargetData);
            builder.In(followCreatureState).On(CharacterEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICharacterData>(inventoryState.SetTargetData);
            
            // Use Consumable
            builder.In(fleeFromCharacterState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
            builder.In(followCreatureState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
            builder.In(wanderState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
            builder.In(fleeFromCharacterState).On(CharacterEvent.Event.OUT_OF_STAMINA).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
            builder.In(wanderState).On(CharacterEvent.Event.OUT_OF_STAMINA).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
            
            // Follow creature
            builder.In(wanderState).On(CharacterEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState).Execute<ICharacterData>(followCreatureState.SetTargetData);
            builder.In(attackState).On(CharacterEvent.Event.CREATURE_OUT_OF_RANGE).Goto(followCreatureState).Execute<ICharacterData>(followCreatureState.SetTargetData);
            
            // Attack creature
            builder.In(followCreatureState).On(CharacterEvent.Event.CREATURE_IN_RANGE).Goto(attackState).Execute<ICharacterData>(attackState.SetTargetData);
            builder.In(attackState).On(CharacterEvent.Event.CREATURE_IN_RANGE).Execute<ICharacterData>(attackState.SetTargetData);
            builder.In(attackState).On(CharacterEvent.Event.OUT_OF_STAMINA).Execute<ICharacterData>(attackState.SetTargetData);
            
            // Flee From creature
            builder.In(attackState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(fleeFromCharacterState).Execute<ICharacterData>(fleeFromCharacterState.SetTargetData);

            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
            
            // Start State machine loop
            Update();
        }
    }
}