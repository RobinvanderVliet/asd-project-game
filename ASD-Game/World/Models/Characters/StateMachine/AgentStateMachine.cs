using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Appccelerate.StateMachine.Machine;
using ASD_Game.World.Models.Characters.StateMachine.Builder;
using ASD_Game.World.Models.Characters.StateMachine.CustomRuleSet;
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
            ruleSet2.Threshold = "monster";
            ruleSet2.Comparison = "nearby";
            ruleSet2.ComparisonTrue = "engage";
            
            // RuleSet ruleSet4 = new RuleSet();
            // ruleSet4.Setting = "combat";
            // ruleSet4.Action = "default";
            // ruleSet4.Comparable = "agent";
            // ruleSet4.Threshold = "monster";
            // ruleSet4.Comparison = "sees";
            // ruleSet4.ComparisonTrue = "follow";
            //
            // RuleSet ruleSet5 = new RuleSet();
            // ruleSet5.Setting = "combat";
            // ruleSet5.Action = "default";
            // ruleSet5.Comparable = "agent";
            // ruleSet5.Threshold = "monster";
            // ruleSet5.Comparison = "nearby";
            // ruleSet5.ComparisonTrue = "engage";

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
                ruleSet3
                // ruleSet4,
                // ruleSet5
            };

            // var rulesetList = RuleSetFactory.GetRuleSetListFromSettingsList(_characterData.RuleSet);

            var builderConfigurator = new BuilderConfigurator(rulesetList, CharacterData, this);

            builderConfigurator.ActionsWithStateList = new()
            {
                new KeyValuePair<string, CharacterState>("idle", idleState),
                new KeyValuePair<string, CharacterState>("inventory", inventoryState),
                new KeyValuePair<string, CharacterState>("flee", fleeFromCharacterState),
                new KeyValuePair<string, CharacterState>("follow", followCreatureState),
                new KeyValuePair<string, CharacterState>("wander", wanderState),
                new KeyValuePair<string, CharacterState>("use", useConsumableState),
                new KeyValuePair<string, CharacterState>("attack", attackState)
            };

            var builderInfoList = builderConfigurator.GetBuilderInfoList();

            CharacterData.BuilderConfigurator = builderConfigurator;

            foreach (BuilderInfo builderInfo in builderInfoList)
            {
                foreach (var initialState in builderInfo.InitialStates)
                {
                    builder.In(initialState).On(builderInfo.Event)
                        .If<object>((targetData) => builderConfigurator.GetGuard(_characterData, targetData, builderInfo))
                        .Goto(builderInfo.TargetState).Execute<ICharacterData>(builderInfo.TargetState.SetTargetData);
                }
            }

            foreach (var action in builderConfigurator.GetActionWithStateList())
            {
                if (!builderInfoList.Any(x => x.Action == action.Key))
                {
                    if (action.Key == "idle")
                    {
                        // Idle
                        builder.In(wanderState).On(CharacterEvent.Event.IDLE).Goto(idleState).Execute<ICharacterData>(wanderState.SetTargetData); //IF not using a Target then remove execute.
                    }
                    else if (action.Key == "wander")
                    {
                        // Wandering
                        builder.In(followCreatureState).On(CharacterEvent.Event.LOST_CREATURE).Goto(wanderState).Execute<ICharacterData>(wanderState.SetTargetData);
                        builder.In(fleeFromCharacterState).On(CharacterEvent.Event.LOST_CREATURE).Goto(wanderState).Execute<ICharacterData>(wanderState.SetTargetData);
                        builder.In(idleState).On(CharacterEvent.Event.WANDERING).Goto(wanderState).Execute<ICharacterData>(wanderState.SetTargetData);
                        builder.In(attackState).On(CharacterEvent.Event.LOST_CREATURE).Goto(wanderState).Execute<ICharacterData>(wanderState.SetTargetData);
                    }
                    else if (action.Key == "inventory")
                    {
                        // Manage inventory
                        builder.In(wanderState).On(CharacterEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICharacterData>(inventoryState.SetTargetData);
                        builder.In(fleeFromCharacterState).On(CharacterEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICharacterData>(inventoryState.SetTargetData);
                        builder.In(followCreatureState).On(CharacterEvent.Event.FOUND_ITEM).Goto(inventoryState).Execute<ICharacterData>(inventoryState.SetTargetData);
                    }
                    else if (action.Key == "consumable")
                    {
                        // Use Consumable
                        builder.In(fleeFromCharacterState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
                        builder.In(followCreatureState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
                        builder.In(wanderState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
                        builder.In(fleeFromCharacterState).On(CharacterEvent.Event.OUT_OF_STAMINA).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
                        builder.In(wanderState).On(CharacterEvent.Event.OUT_OF_STAMINA).Goto(useConsumableState).Execute<ICharacterData>(useConsumableState.SetTargetData);
                    }
                    else if (action.Key == "follow")
                    {
                        // Follow creature
                        builder.In(wanderState).On(CharacterEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState).Execute<ICharacterData>(followCreatureState.SetTargetData);
                    }
                    else if (action.Key == "attack")
                    {
                        // Attack creature
                        builder.In(followCreatureState).On(CharacterEvent.Event.CREATURE_IN_RANGE).Goto(attackState).Execute<ICharacterData>(attackState.SetTargetData);
                        builder.In(attackState).On(CharacterEvent.Event.CREATURE_IN_RANGE).Execute<ICharacterData>(attackState.SetTargetData);
                        builder.In(attackState).On(CharacterEvent.Event.OUT_OF_STAMINA).Execute<ICharacterData>(attackState.SetTargetData);
                    }
                    else if (action.Key == "flee")
                    {
                        // Flee From creature
                        builder.In(attackState).On(CharacterEvent.Event.ALMOST_DEAD).Goto(fleeFromCharacterState).Execute<ICharacterData>(fleeFromCharacterState.SetTargetData);
                    }
                }
            }

            // Follow creature
            // builder.In(wanderState).On(CharacterEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState).Execute<ICharacterData>(followCreatureState.SetTargetData);
            // builder.In(followCreatureState).On(CharacterEvent.Event.SPOTTED_CREATURE).Goto(followCreatureState).Execute<ICharacterData>(followCreatureState.SetTargetData);
            //
            // builder.In(wanderState).On(CharacterEvent.Event.CREATURE_IN_RANGE).Goto(attackState).Execute<ICharacterData>(attackState.SetTargetData);
            // builder.In(attackState).On(CharacterEvent.Event.CREATURE_IN_RANGE).Goto(attackState).Execute<ICharacterData>(attackState.SetTargetData);
            // builder.In(followCreatureState).On(CharacterEvent.Event.CREATURE_IN_RANGE).Goto(attackState).Execute<ICharacterData>(attackState.SetTargetData);

            builder.WithInitialState(wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
            
            // Start State machine loop
            Update();
        }
    }
}