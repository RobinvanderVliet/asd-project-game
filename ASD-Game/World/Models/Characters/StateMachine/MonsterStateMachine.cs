using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Appccelerate.StateMachine.Machine;
using ASD_Game.World.Models.Characters.StateMachine.Builder;
using Creature.Creature.StateMachine.CustomRuleSet;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;
using WorldGeneration.StateMachine.CustomRuleSet;
using World.Models.Characters.StateMachine.Event;

namespace ASD_Game.World.Models.Characters.StateMachine
{
    public class MonsterStateMachine : DefaultStateMachine
    {
        private Timer _timer;

        public MonsterStateMachine(ICharacterData characterData) : base(characterData)
        {
        }

        [ExcludeFromCodeCoverage]
        public ICharacterData CharacterData
        {
            get => _characterData;
            set => _characterData = (MonsterData) value;
        }

        public override void StartStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<CharacterState, CharacterEvent.Event>();
            var ruleSetFactory = new RuleSetFactory();
            //var rulesetList = ruleSetFactory.GetRuleSetListFromSettingsList(CharacterData.RuleSet);

            RuleSet ruleSet1 = new RuleSet();
            ruleSet1.Setting = "combat";
            ruleSet1.Action = "default";
            ruleSet1.Comparable = "agent";
            ruleSet1.Threshold = "monster";
            ruleSet1.Comparison = "sees";
            ruleSet1.ComparisonTrue = "follow";

            RuleSet ruleSet2 = new RuleSet();
            ruleSet2.Setting = "combat";
            ruleSet2.Action = "default";
            ruleSet2.Comparable = "agent";
            ruleSet2.Threshold = "agent";
            ruleSet2.Comparison = "nearby";
            ruleSet2.ComparisonTrue = "engage";

            RuleSet ruleSet3 = new RuleSet();
            ruleSet3.Setting = "combat";
            ruleSet3.Action = "engage";
            ruleSet3.Comparable = "health";
            ruleSet3.Threshold = "50";
            ruleSet3.Comparison = "less than";
            ruleSet3.ComparisonTrue = "flee";
            ruleSet3.ComparisonFalse = "attack";

            List<RuleSet> rulesetList = new()
            {
                ruleSet1,
                ruleSet2,
                ruleSet3,
            };

            var builderConfigurator = new BuilderConfigurator(rulesetList, CharacterData, this);
            var builderInfoList = builderConfigurator.GetBuilderInfoList();

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

            //foreach (BuilderInfo builderInfo in builderInfoList)
            //{
            //    foreach (var initialState in builderInfo.InitialStates)
            //    {
            //        builder.In(initialState).On(builderInfo.Event)
            //            .If<object>((targetData) => builderConfigurator.GetGuard(_characterData, targetData, builderInfo))
            //            .Goto(builderInfo.TargetState).Execute<ICharacterData>(builderInfo.TargetState.SetTargetData);
            //    }
            //}

            //foreach (var action in builderConfigurator.GetActionWithStateList())
            //{
            //    if (!builderInfoList.Any(x => x.Action == action.Key))
            //    {
            //        if (action.Key == "wander")
            //        {
            //            // Wandering
            //            builder.In(followCreatureState).On(CharacterEvent.Event.LOST_CREATURE).Goto(wanderState);
            //        }
            //        else if (action.Key == "follow")
            //        {
            //            // Follow player
            //            builder.In(wanderState).On(CharacterEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState)
            //                .Execute<ICharacterData>(followCreatureState.SetTargetData);
            //            builder.In(followCreatureState).On(CharacterEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState)
            //                .Execute<ICharacterData>(followCreatureState.SetTargetData);
            //            builder.In(attackState).On(CharacterEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState)
            //                .Execute<ICharacterData>(followCreatureState.SetTargetData);
            //        }
            //        else if (action.Key == "attack")
            //        {
            //            // Attack player
            //            builder.In(followCreatureState).On(CharacterEvent.Event.CREATURE_IN_RANGE)
            //                .Goto(attackState).Execute<ICharacterData>(attackState.SetTargetData);
            //            builder.In(attackState).On(CharacterEvent.Event.CREATURE_IN_RANGE)
            //                .Execute<ICharacterData>(attackState.SetTargetData);
            //        }
            //        //else if (action == "...")
            //    }
            //}

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