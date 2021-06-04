using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.Builder;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;
using Creature.Creature.StateMachine.Event;
using Creature.Creature.StateMachine.State;
using System.Collections.Generic;
using System.Linq;

namespace ASD_project.Creature.Creature.StateMachine.Builder
{
    public class BuilderConfiguration
    {
        List<RuleSet> _rulesetList;
        ICreatureData _creatureData;
        ICreatureStateMachine _stateMachine;

        public BuilderConfiguration(List<RuleSet> rulesetList, ICreatureData creatureData, ICreatureStateMachine stateMachine)
        {
            _rulesetList = rulesetList;
            _creatureData = creatureData;
            _stateMachine = stateMachine;
        }

        public List<BuilderInfo> GetBuilderInfoList()
        {
            List<BuilderInfo> builderInfoList = new List<BuilderInfo>();

            foreach (var action in GetActionWithStateList())
            {
                foreach (RuleSet ruleSet in _rulesetList)
                {
                    if (ruleSet.ComparisonTrue == action.Key || ruleSet.ComparisonFalse == action.Key)
                    {
                        BuilderInfo builderInfo = new();
                        builderInfo.Action = action.Key;
                        builderInfo.TargetState = action.Value;

                        // Temporary
                        builderInfo.InitialStates = GetActionWithStateList().Select(state => state.Value).ToList();

                        if (ruleSet.Action == "default")
                        {
                            CreatureEvent.Event creatureEvent = GetEvent(ruleSet, action.Key);
                            builderInfo.Event = creatureEvent;
                            builderInfo.RuleSets.Add(ruleSet);
                        }
                        else
                        {
                            foreach (RuleSet ruleSet2 in _rulesetList)
                            {
                                if (ruleSet2.Action == "default" && (ruleSet2.ComparisonTrue == ruleSet.Action || ruleSet2.ComparisonFalse == ruleSet.Action))
                                {
                                    CreatureEvent.Event creatureEvent = GetEvent(ruleSet2, action.Key);
                                    builderInfo.RuleSets.Add(ruleSet2);
                                    builderInfo.RuleSets.Add(ruleSet);
                                }
                            }
                        }

                        builderInfoList.Add(builderInfo);
                    }
                }
            }
            return builderInfoList;
        }

        public CreatureEvent.Event GetEvent(RuleSet rule, string state)
        {
            if ((rule.Comparable == "monster" || rule.Comparable == "agent") && (rule.Threshold == "monster" || rule.Threshold == "agent"))
            {
                if (rule.Comparison == "sees")
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

        public bool GetGuard(object comparableData, object thresholdData, List<RuleSet> ruleSets, string state)
        {
            bool firstRulesetCondition = true;

            if (ruleSets[0].Comparison == "sees" || ruleSets[0].Comparison == "nearby" || ruleSets[0].Comparison == "lost")
            {
                firstRulesetCondition = IsInstanceOf(comparableData, ruleSets[0].Comparable) && IsInstanceOf(thresholdData, ruleSets[0].Threshold);
            }

            bool secondRulesetCondition = true;

            if (ruleSets.Count > 1)
            {
                object comparableObject = GetData(comparableData, ruleSets[1].Comparable);
                object thresholdObject = GetData(thresholdData, ruleSets[1].Threshold);

                if (ruleSets[1].Comparison == "less than")
                {
                    secondRulesetCondition = (int)comparableObject < (int)thresholdObject;
                }
                else if (ruleSets[1].Comparison == "greater than")
                {
                    secondRulesetCondition = (int)comparableObject > (int)thresholdObject;
                }
                else if (ruleSets[1].Comparison == "is equal to")
                {
                    secondRulesetCondition = comparableObject == thresholdObject;
                }
                else if (ruleSets[1].Comparison == "contains")
                {
                    // return ?
                }
                else if (ruleSets[1].Comparison == "does not contain")
                {
                    // return ?
                }

                if (ruleSets[1].ComparisonFalse == state)
                {
                    secondRulesetCondition = !secondRulesetCondition;
                }
            }

            if (firstRulesetCondition == false || secondRulesetCondition == false)
            {
                return false;
            }
            return true;
        }

        public object GetData(object comparisonData, string comparisonString)
        {
            if (comparisonString == "health")
            {
                ICreatureData data = (ICreatureData)comparisonData;
                return data.Health;
            }
            else if (comparisonString == "stamina")
            {
                AgentData data = (AgentData)comparisonData;
                return data.Stamina;
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
            else if (comparisonString == "opponent")
            {

            }
            // TODO: Add more

            return comparisonData;
        }

        public bool IsInstanceOf(object comparisonData, string comparisonString)
        {
            if (comparisonString == "agent")
            {
                if (comparisonData.GetType() == typeof(AgentData))
                {
                    return true;
                }
            }
            else if (comparisonString == "monster")
            {
                if (comparisonData.GetType() == typeof(MonsterData))
                {
                    return true;
                }
            }
            return false;
        }

        public List<KeyValuePair<string, CreatureState>> GetActionWithStateList()
        {
            return new List<KeyValuePair<string, CreatureState>>
            {
                new("engage", new EngageState(_creatureData, _stateMachine)),
                new("collect", new CollectState(_creatureData, _stateMachine)),
                new("follow", new FollowCreatureState(_creatureData, _stateMachine)),
                new("flee", new FleeFromCreatureState(_creatureData, _stateMachine)),
                new("attack", new AttackState(_creatureData, _stateMachine)),
                new("idle", new IdleState(_creatureData, _stateMachine)),
                new("inventory", new InventoryState(_creatureData, _stateMachine)),
                new("wander", new WanderState(_creatureData, _stateMachine))
            };
        }
    }
}
