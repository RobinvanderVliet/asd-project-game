using System.Linq;
using System.Timers;
using Appccelerate.StateMachine.Machine;
using ASD_Game.World.Models.Characters.StateMachine.Builder;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;
using Creature.Creature.StateMachine.CustomRuleSet;
using World.Models.Characters.StateMachine.Event;

namespace ASD_Game.World.Models.Characters.StateMachine
{
    public class AgentStateMachine : DefaultStateMachine
    {
        private Timer _timer;

        public AgentStateMachine(ICharacterData characterData) : base(characterData)
        {
        }

        public new ICharacterData CharacterData
        {
            get => _characterData;
            set => _characterData = (AgentData) value;
        }

        public override void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CharacterState, CharacterEvent.Event>();
            var rulesetList = RuleSetFactory.GetRuleSetListFromSettingsList(CharacterData.RuleSet);
            var builderConfigurator = new BuilderConfigurator(rulesetList, CharacterData, this);

            CharacterData.BuilderConfigurator = builderConfigurator;

            CharacterState idleState = new IdleState(CharacterData, this);
            CharacterState inventoryState = new InventoryState(CharacterData, this);
            CharacterState fleeFromCharacterState = new FleeFromCreatureState(CharacterData, this);
            CharacterState followCreatureState = new FollowCreatureState(CharacterData, this);
            CharacterState wanderState = new WanderState(CharacterData, this);
            CharacterState useConsumableState = new UseConsumableState(CharacterData, this);
            CharacterState attackState = new AttackState(CharacterData, this);
                
            DefineDefaultBehaviour(ref builder, ref followCreatureState);
            DefineDefaultBehaviour(ref builder, ref wanderState);
            DefineDefaultBehaviour(ref builder, ref useConsumableState);
            DefineDefaultBehaviour(ref builder, ref attackState);
            DefineDefaultBehaviour(ref builder, ref idleState);
            DefineDefaultBehaviour(ref builder, ref inventoryState);
            DefineDefaultBehaviour(ref builder, ref fleeFromCharacterState);

            // foreach (BuilderInfo builderInfo in builderInfoList)
            // {
            //     foreach (var initialState in builderInfo.InitialStates)
            //     {
            //         builder.In(initialState).On(builderInfo.Event)
            //             .If<object>((targetData) => builderConfigurator.GetGuard(_characterData, targetData, builderInfo))
            //             .Goto(builderInfo.TargetState).Execute<ICharacterData>(builderInfo.TargetState.SetTargetData);
            //     }
            // }
            //
            // foreach (var action in builderConfigurator.GetActionWithStateList())
            // {
            //     if (!builderInfoList.Any(x => x.Action == action.Key))
            //     {
            //         if (action.Key == "idle")
            //         {
            //             // Idle
            //             builder.In(wanderState).On(CharacterEvent.Event.IDLE).Goto(idleState).Execute<ICharacterData>(wanderState.SetTargetData); //IF not using a Target then remove execute.
            //         }
            //         else if (action.Key == "wander")
            //         {
            //             // Wandering
            //             builder.In(followCreatureState).On(CharacterEvent.Event.LOST_CREATURE).Goto(wanderState).Execute<ICharacterData>(wanderState.SetTargetData);
            //             builder.In(fleeFromCharacterState).On(CharacterEvent.Event.LOST_CREATURE).Goto(wanderState).Execute<ICharacterData>(wanderState.SetTargetData);
            //             builder.In(idleState).On(CharacterEvent.Event.WANDERING).Goto(wanderState).Execute<ICharacterData>(wanderState.SetTargetData);
            //         }
            //         else if (action.Key == "inventory")
            //         {
            //             // Manage inventory
            //             builder.In(wanderState).On(CharacterEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICharacterData>(inventoryState.SetTargetData);
            //             builder.In(fleeFromCharacterState).On(CharacterEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICharacterData>(inventoryState.SetTargetData);
            //             builder.In(followCreatureState).On(CharacterEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICharacterData>(inventoryState.SetTargetData);
            //         }
            //         else if (action.Key == "consumable")
            //         {
            //             // Use Consumable
            //             builder.In(fleeFromCharacterState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
            //             builder.In(followCreatureState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
            //             builder.In(wanderState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
            //             builder.In(fleeFromCharacterState).On(CharacterEvent.Event.OUT_OF_STAMINA).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
            //             builder.In(wanderState).On(CharacterEvent.Event.OUT_OF_STAMINA).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
            //         }
            //         else if (action.Key == "follow")
            //         {
            //             // Follow creature
            //             builder.In(wanderState).On(CharacterEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState).Execute<ICharacterData>(followCreatureState.SetTargetData);
            //             builder.In(attackState).On(CharacterEvent.Event.CREATURE_OUT_OF_RANGE).Goto(followCreatureState).Execute<ICharacterData>(followCreatureState.SetTargetData);
            //         }
            //         else if (action.Key == "attack")
            //         {
            //             // Attack creature
            //             builder.In(followCreatureState).On(CharacterEvent.Event.CREATURE_IN_RANGE).Goto(attackState).Execute<ICharacterData>(attackState.SetTargetData);
            //             builder.In(attackState).On(CharacterEvent.Event.CREATURE_IN_RANGE).Execute<ICharacterData>(attackState.SetTargetData);
            //             builder.In(attackState).On(CharacterEvent.Event.OUT_OF_STAMINA).Execute<ICharacterData>(attackState.SetTargetData);
            //         }
            //         else if (action.Key == "flee")
            //         {
            //             // Flee From creature
            //             builder.In(attackState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(fleeFromCharacterState).Execute<ICharacterData>(fleeFromCharacterState.SetTargetData);
            //         }
            //     }
            // }

            // Follow creature
            builder.In(wanderState).On(CharacterEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState).Execute<ICharacterData>(followCreatureState.SetTargetData);
            builder.In(followCreatureState).On(CharacterEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState).Execute<ICharacterData>(followCreatureState.SetTargetData);
            builder.In(wanderState).On(CharacterEvent.Event.CREATURE_IN_RANGE).Goto(followCreatureState).Execute<ICharacterData>(followCreatureState.SetTargetData);
            builder.In(followCreatureState).On(CharacterEvent.Event.CREATURE_IN_RANGE).Goto(followCreatureState).Execute<ICharacterData>(followCreatureState.SetTargetData);


            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
            
            // Start State machine loop
            Update();
        }
    }
}