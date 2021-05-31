using System.Collections.Generic;
using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;

namespace Creature.Creature.StateMachine
{
    class MonsterStateMachine : ICreatureStateMachine
    {
        private PassiveStateMachine<CreatureState, CreatureEvent.Event> _passiveStateMachine;
        private MonsterData _monsterData;

        private CreatureState _followPlayerState;
        private CreatureState _wanderState;
        private CreatureState _useConsumableState;
        private CreatureState _attackPlayerState;
        private CreatureState _fleeFromCreatureState;

        public MonsterStateMachine(MonsterData monsterData)
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

            List<RuleSet> rulesetList = RuleSetFactory.GetRuleSetListFromDictionaryList(CreatureData.RuleSet);

            CreatureEvent.Event followEvent = CreatureEvent.Event.IDLE;
            bool followGuard = false;

            RuleSet rule = new RuleSet();

            // Check for each state (in _comparison_true of _false) if they're default or not;
            // If default, get event
            // If not default, go to default and get event
            foreach (RuleSet ruleSet in rulesetList)
            {
                //builder.In(GetFirstState(CreatureData)).On(GetEvent(CreatureData)).If(GetGuard(ruleSet)).Execute(GetSecondState(CreatureData);

                if (ruleSet.ComparisonTrue == "follow" || ruleSet.ComparisonFalse == "follow")
                {
                    if (ruleSet.Action == "default")
                    {
                        if (ruleSet.ComparisonTrue == "follow")
                        {
                            followEvent = GetEvent(ruleSet.Comparable, ruleSet.Threshold, ruleSet.Comparison);
                        }
                    }
                    else
                    {
                        foreach (RuleSet ruleSet2 in rulesetList)
                        {
                            if (ruleSet2.Action == "default" && (ruleSet2.ComparisonTrue == "follow" || ruleSet2.ComparisonFalse == "follow"))
                            {
                                followEvent = GetEvent(ruleSet2.Comparable, ruleSet2.Threshold, ruleSet2.Comparison);
                                rule = ruleSet2;
                            }
                        }
                    }
                }
            }

            // Test
            builder.In(_followPlayerState).On(followEvent).
                If<ICreatureData>((c) => GetGuard(CreatureData, c, rule)).
                Goto(_attackPlayerState).Execute<ICreatureData>(new AttackState(CreatureData).Do);

            // Follow player
            builder.In(_wanderState).On(CreatureEvent.Event.SPOTTED_CREATURE).
                Goto(_followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            builder.In(_followPlayerState).On(CreatureEvent.Event.SPOTTED_CREATURE).Goto(_followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);
            builder.In(_attackPlayerState).On(CreatureEvent.Event.SPOTTED_CREATURE).Goto(_followPlayerState).Execute<ICreatureData>(new FollowCreatureState(CreatureData).Do);

            // Attack player
            builder.In(_followPlayerState).On(CreatureEvent.Event.CREATURE_IN_RANGE).Goto(_attackPlayerState).Execute<ICreatureData>(new AttackState(CreatureData).Do);
            builder.In(_attackPlayerState).On(CreatureEvent.Event.CREATURE_IN_RANGE).Execute<ICreatureData>(new AttackState(CreatureData).Do);

            // Use potion
            builder.In(_attackPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(_useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);
            builder.In(_followPlayerState).On(CreatureEvent.Event.ALMOST_DEAD).Goto(_useConsumableState).Execute<ICreatureData>(new UseConsumableState(CreatureData).Do);

            builder.WithInitialState(_wanderState);

            _passiveStateMachine = builder.Build().CreatePassiveStateMachine();
            _passiveStateMachine.Start();
        }
        public CreatureEvent.Event GetEvent(string comparable, string threshold, string comparison)
        {
            if ((comparable == "monster" || comparable == "agent") && (threshold == "monster" || threshold == "agent"))
            {
                if (comparison == "sees" || comparison == "finds")
                {
                    return CreatureEvent.Event.SPOTTED_CREATURE;
                }
                else if (comparison == "nearby")
                {
                    return CreatureEvent.Event.CREATURE_IN_RANGE;
                }
                else if (comparison == "lost")
                {
                    return CreatureEvent.Event.LOST_CREATURE;
                }
            }
            else if ((comparable == "monster" || comparable == "agent") && threshold == "item")
            {
                if (comparison == "finds")
                {
                    return CreatureEvent.Event.FOUND_ITEM;
                }
            }
            return CreatureEvent.Event.IDLE;
        }

        public bool GetGuard(ICreatureData comparableData, ICreatureData thresholdData, RuleSet rule)
        {
            object comparableObject = new();
            object thresholdObject = new();

            if (rule.Comparable == "agent" || rule.Comparable == "monster")
            {
                comparableObject = comparableData;
            }
            else if (rule.Threshold == "agent" || rule.Threshold == "monster")
            {
                thresholdObject = thresholdData;
            }
            else if (rule.Comparable == "health")
            {
                comparableObject = comparableData.Health;
            }
            else if (rule.Threshold == "health")
            {
                thresholdObject = thresholdData.Health;
            }

            bool condition = true;

            if (rule.Comparison == "less than")
            {
                condition = ((int)comparableObject < (int)thresholdObject);
            }
            else if (rule.Comparison == "greater than")
            {
                condition = (int)comparableObject > (int)thresholdObject;
            }
            else if (rule.Comparison == "is equal to")
            {
                condition = (int)comparableObject == (int)thresholdObject;
            }
            else if (rule.Comparison == "contains")
            {
                //return ???
            }
            else if (rule.Comparison == "does not contain")
            {
                //return ???
            }

            // TODO: invert condition value if action gets executed when comparison is false

            return condition;
        }
    }
}