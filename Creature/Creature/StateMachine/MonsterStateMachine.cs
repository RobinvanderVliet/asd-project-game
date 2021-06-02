using System.Collections.Generic;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.State;
using System.Linq;

namespace Creature.Creature.StateMachine
{
    class MonsterStateMachine : ICreatureStateMachine
    {
        private PassiveStateMachine<CreatureState, CreatureEvent.Event> _passiveStateMachine;
        private ICreatureData _monsterData;

        private CreatureState _followPlayerState;
        private CreatureState _wanderState;
        private CreatureState _useConsumableState;
        private CreatureState _attackPlayerState;
        private CreatureState _fleeFromCreatureState;

        public MonsterStateMachine(ICreatureData monsterData)
        {
            _monsterData = monsterData;
        }

        public ICreatureData CreatureData
        {
            get => _monsterData;
            set => _monsterData = (MonsterData) value;
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
            
            _followPlayerState = new FollowCreatureState(CreatureData);
            _wanderState = new WanderState(CreatureData);
            _useConsumableState = new UseConsumableState(CreatureData);
            _attackPlayerState = new AttackState(CreatureData);
            _fleeFromCreatureState = new FleeFromCreatureState(CreatureData);

            // Wandering
            builder.In(_followPlayerState).On(CreatureEvent.Event.LOST_CREATURE).Goto(_wanderState);

            // TODO: Move statemachine builder info outside of statemachine classes
            List<RuleSet> rulesetList = RuleSetFactory.GetRuleSetListFromSettingsList(CreatureData.RuleSet);
            List<string> actions = new() { "follow", "flee", "attack" };
            List<BuilderInfo> builderInfoList = new List<BuilderInfo>();

            foreach (var action in actions)
            {
                foreach (RuleSet ruleSet in rulesetList)
                {
                    if (ruleSet.ComparisonTrue == action || ruleSet.ComparisonFalse == action)
                    {
                        BuilderInfo builderInfo = new();
                        builderInfo.Action = action;
                        builderInfo.TargetState = GetTargetState(action);

                        // Temporary
                        builderInfo.InitialStates = GetInitialStates();

                        if (ruleSet.Action == "default")
                        {
                            CreatureEvent.Event creatureEvent = GetEvent(ruleSet, action);
                            builderInfo.Event = creatureEvent;
                            builderInfo.RuleSets.Add(ruleSet);
                        }
                        else
                        {
                            foreach (RuleSet ruleSet2 in rulesetList)
                            {
                                if (ruleSet2.Action == "default" && (ruleSet2.ComparisonTrue == ruleSet.Action || ruleSet2.ComparisonFalse == ruleSet.Action))
                                {
                                    CreatureEvent.Event creatureEvent = GetEvent(ruleSet2, action);
                                    if (creatureEvent == CreatureEvent.Event.IDLE)
                                    {
                                        creatureEvent = GetEvent(ruleSet, action);
                                    }
                                    builderInfo.RuleSets.Add(ruleSet);
                                    builderInfo.RuleSets.Add(ruleSet2);
                                }
                            }
                        }

                        builderInfoList.Add(builderInfo);
                    }
                }
            }

            foreach (BuilderInfo builderInfo in builderInfoList)
            {
                foreach (var initialState in builderInfo.InitialStates)
                {
                    builder.In(initialState).On(builderInfo.Event).
                        If<object>((targetData) => builderInfo.RuleSets.All(ruleSet => GetGuard(CreatureData, targetData, ruleSet, builderInfo.Action))).
                        Goto(builderInfo.TargetState).Execute<ICreatureData>(builderInfo.TargetState.SetTargetData);
                }
            }

            foreach (var action in actions)
            {
                if (!builderInfoList.Any(x => x.Action == action))
                {
                    // TODO: If action not used in builder info, do default stuff for it like below
                }
            }

            // Follow player
            builder.In(_wanderState).On(CreatureEvent.Event.SPOTTED_CREATURE).
                Goto(_followPlayerState).Execute<ICreatureData>(_followPlayerState.SetTargetData);
            builder.In(_followPlayerState).On(CreatureEvent.Event.SPOTTED_CREATURE).Goto(_followPlayerState).Execute<ICreatureData>(_followPlayerState.SetTargetData);
            builder.In(_attackPlayerState).On(CreatureEvent.Event.SPOTTED_CREATURE).Goto(_followPlayerState).Execute<ICreatureData>(_followPlayerState.SetTargetData);

            // Attack player
            builder.In(_followPlayerState).On(CreatureEvent.Event.CREATURE_IN_RANGE).Goto(_attackPlayerState).Execute<ICreatureData>(_attackPlayerState.SetTargetData);
            builder.In(_attackPlayerState).On(CreatureEvent.Event.CREATURE_IN_RANGE).Execute<ICreatureData>(_attackPlayerState.SetTargetData);

            // Use potion
            builder.In(_attackPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(_useConsumableState).Execute<ICreatureData>(_useConsumableState.SetTargetData);
            builder.In(_followPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(_useConsumableState).Execute<ICreatureData>(_useConsumableState.SetTargetData);

            builder.WithInitialState(_wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
        }

        private List<CreatureState> GetInitialStates()
        {
            List<CreatureState> states = new()
            {
                _followPlayerState,
                _wanderState,
                _useConsumableState,
                _attackPlayerState,
                _fleeFromCreatureState
            };
            return states;
        }

        private CreatureEvent.Event GetEvent(RuleSet rule, string state)
        {
            if ((rule.Comparable == "monster" || rule.Comparable == "agent") && (rule.Threshold == "monster" || rule.Threshold == "agent"))
            {
                if (rule.Comparison == "sees" || rule.Comparison == "finds")
                {
                    if (rule.ComparisonFalse == state)
                    {
                        return CreatureEvent.Event.IDLE;
                    }
                    return CreatureEvent.Event.SPOTTED_CREATURE;
                }
                else if (rule.Comparison == "nearby")
                {
                    if (rule.ComparisonFalse == state)
                    {
                        return CreatureEvent.Event.SPOTTED_CREATURE;
                    }
                    return CreatureEvent.Event.CREATURE_IN_RANGE;
                }
                else if (rule.Comparison == "lost")
                {
                    if (rule.ComparisonFalse == state)
                    {
                        return CreatureEvent.Event.SPOTTED_CREATURE;
                    }
                    return CreatureEvent.Event.LOST_CREATURE;
                }
            }
            else if ((rule.Comparable == "monster" || rule.Comparable == "agent") && rule.Threshold == "item")
            {
                if (rule.Comparison == "finds")
                {
                    if (rule.ComparisonFalse == state)
                    {
                        return CreatureEvent.Event.IDLE;
                    }
                    return CreatureEvent.Event.FOUND_ITEM;
                }
            }
            // TODO: Add more

            return CreatureEvent.Event.IDLE;
        }

        private bool GetGuard(object comparableData, object thresholdData, RuleSet rule, string state)
        {
            object comparableObject = GetData(comparableData, rule.Comparable);
            object thresholdObject = GetData(thresholdData, rule.Threshold);

            bool condition = true;

            if (rule.Comparison == "less than")
            {
                condition = (int)comparableObject < (int)thresholdObject;
            }
            else if (rule.Comparison == "greater than")
            {
                condition = (int)comparableObject > (int)thresholdObject;
            }
            else if (rule.Comparison == "is equal to")
            {
                condition = comparableObject == thresholdObject;
            }
            else if (rule.Comparison == "contains")
            {
                // return ?
            }
            else if (rule.Comparison == "does not contain")
            {
                // return ?
            }

            if (rule.ComparisonFalse == state)
            {
                condition = !condition;
            }

            return condition;
        }

        private object GetData(object comparisonData, string comparisonString)
        {
            if (comparisonString == "agent" || comparisonString == "monster")
            {
                ICreatureData data = (ICreatureData)comparisonData;
                return data;
            }
            else if (comparisonString == "health")
            {
                ICreatureData data = (ICreatureData)comparisonData;
                return data.Health;
            }
            else if (comparisonString == "stamina")
            {
                ICreatureData data = (ICreatureData)comparisonData;
                // TODO: return data.Stamina;
            }
            else if (comparisonString == "inventory")
            {
                ICreatureData data = (ICreatureData)comparisonData;
                // TODO: return data.Inventory;
            }
            else if (int.TryParse(comparisonString, out _))
            {
                return comparisonString;
            }
            // TODO: Add more

            return comparisonData;
        }

        private CreatureState GetTargetState(string action)
        {
            if (action == "follow")
            {
                return new FollowCreatureState(CreatureData);
            }
            else if (action == "flee")
            {
                return new FleeFromCreatureState(CreatureData);
            }
            else if (action == "attack")
            {
                return new AttackState(CreatureData);
            }
            // TODO: Add more
            else 
            {
                return new IdleState(CreatureData);
            }
        }
    }
}