using System;
using System.Collections.Generic;
using System.Linq;
using ASD_Game.Items;
using ASD_Game.World.Models.Characters.StateMachine.CustomRuleSet;
using ASD_Game.World.Models.Characters.StateMachine.Data;
using ASD_Game.World.Models.Characters.StateMachine.State;
using World.Models.Characters.StateMachine.Event;

namespace ASD_Game.World.Models.Characters.StateMachine.Builder
{
    public class BuilderConfigurator
    {
        private readonly List<RuleSet> _rulesetList;
        private readonly ICharacterData _characterData;
        private readonly ICharacterStateMachine _stateMachine;

        public List<KeyValuePair<string, CharacterState>> ActionsWithStateList;

        public BuilderConfigurator(List<RuleSet> rulesetList, ICharacterData characterData, ICharacterStateMachine stateMachine)
        {
            _rulesetList = rulesetList;
            _characterData = characterData;
            _stateMachine = stateMachine;
            ActionsWithStateList = new();
        }

        public List<BuilderInfo> GetBuilderInfoList()
        {
            List<BuilderInfo> builderInfoList = new List<BuilderInfo>();

            foreach (var action in ActionsWithStateList)
            {
                foreach (RuleSet ruleSet in _rulesetList)
                {
                    if (ruleSet.ComparisonTrue == action.Key || ruleSet.ComparisonFalse == action.Key)
                    {
                        BuilderInfo builderInfo = new();
                        builderInfo.Action = action.Key;
                        builderInfo.TargetState = action.Value;

                        builderInfo.InitialStates = ActionsWithStateList.Select(state => state.Value).ToList();

                        if (ruleSet.Action == "default")
                        {
                            CharacterEvent.Event CharacterEvent = GetEvent(ruleSet, action.Key);
                            builderInfo.Event = CharacterEvent;
                            builderInfo.RuleSets.Add(ruleSet);
                            builderInfoList.Add(builderInfo);
                        }
                        else
                        {
                            foreach (RuleSet ruleSet2 in _rulesetList)
                            {
                                if (ruleSet2.Setting == ruleSet.Setting && 
                                    ruleSet2.Action == "default" && 
                                    (ruleSet2.ComparisonTrue == ruleSet.Action || ruleSet2.ComparisonFalse == ruleSet.Action))
                                {
                                    builderInfo = new();
                                    builderInfo.Action = action.Key;
                                    builderInfo.TargetState = action.Value;

                                    // Temporary
                                    builderInfo.InitialStates = ActionsWithStateList.Select(state => state.Value).ToList();

                                    CharacterEvent.Event CharacterEvent = GetEvent(ruleSet2, ruleSet.Action);
                                    builderInfo.Event = CharacterEvent;
                                    builderInfo.RuleSets.Add(ruleSet2);
                                    builderInfo.RuleSets.Add(ruleSet);
                                    builderInfoList.Add(builderInfo);
                                }
                            }
                        }
                    }
                }
            }
            return builderInfoList;
        }

        private CharacterEvent.Event GetEvent(RuleSet rule, string state)
        {
            if ((rule.Comparable == "monster" || rule.Comparable == "agent") && (rule.Threshold == "monster" || rule.Threshold == "agent" || rule.Threshold == "player"))
            {
                if (rule.Comparison == "sees")
                {
                    if (rule.ComparisonFalse == state)
                    {
                        return CharacterEvent.Event.IDLE;
                    }
                    return CharacterEvent.Event.SPOTTED_CREATURE;
                }
                else if (rule.Comparison == "nearby")
                {
                    if (rule.ComparisonFalse == state)
                    {
                        return CharacterEvent.Event.SPOTTED_CREATURE;
                    }
                    return CharacterEvent.Event.CREATURE_IN_RANGE;
                }
                else if (rule.Comparison == "lost")
                {
                    if (rule.ComparisonFalse == state)
                    {
                        return CharacterEvent.Event.SPOTTED_CREATURE;
                    }
                    return CharacterEvent.Event.LOST_CREATURE;
                }
            }
            else if (rule.Comparable == "agent" && rule.Threshold == "item")
            {
                if (rule.Comparison == "finds")
                {
                    if (rule.ComparisonFalse == state)
                    {
                        return CharacterEvent.Event.IDLE;
                    }
                    return CharacterEvent.Event.FOUND_ITEM;
                }
            }

            return CharacterEvent.Event.IDLE;
        }

        public bool GetGuard(object comparableData, object thresholdData, BuilderInfo builderInfo)
        {
            bool firstRulesetCondition = true;

            // This checks if the creature that interacts with another creature is of the right instance (because the events for it aren't specific for a monster or an agent)
            if (builderInfo.RuleSets[0].Comparison == "sees" || builderInfo.RuleSets[0].Comparison == "nearby" || builderInfo.RuleSets[0].Comparison == "lost")
            {
                firstRulesetCondition = IsInstanceOf(comparableData, builderInfo.RuleSets[0].Comparable) && IsInstanceOf(thresholdData, builderInfo.RuleSets[0].Threshold);
            }

            bool secondRulesetCondition = true;

            if (builderInfo.RuleSets.Count > 1)
            {
                object comparableObject = GetData(comparableData, builderInfo.RuleSets[1].Comparable);
                object thresholdObject = GetData(thresholdData, builderInfo.RuleSets[1].Threshold);

                if (builderInfo.RuleSets[1].Comparison == "less than")
                {
                    secondRulesetCondition = Convert.ToInt32(comparableObject) < Convert.ToInt32(thresholdObject);
                }
                else if (builderInfo.RuleSets[1].Comparison == "greater than")
                {
                    secondRulesetCondition = Convert.ToInt32(comparableObject) > Convert.ToInt32(thresholdObject);
                }
                else if (builderInfo.RuleSets[1].Comparison == "is equal to")
                {
                    secondRulesetCondition = Convert.ToInt32(comparableObject) == Convert.ToInt32(thresholdObject);
                }
                else if (builderInfo.RuleSets[1].Comparison == "contains")
                {
                    // This checks if the creature that has inventory data is an agent (because a monster can't have an inventory)
                    secondRulesetCondition = IsInstanceOf(comparableData, "agent");
                    if (secondRulesetCondition == true)
                    {
                        Inventory inventory = (Inventory)comparableObject;
                        Item item = (Item)thresholdObject;
                        secondRulesetCondition = inventory.GetConsumableItem(item.ItemName) != null;
                    }
                }
                else if (builderInfo.RuleSets[1].Comparison == "does not contain")
                {
                    // This checks if the creature that has inventory data is an agent (because a monster can't have an inventory)
                    secondRulesetCondition = IsInstanceOf(comparableData, "agent");
                    if (secondRulesetCondition == true)
                    {
                        Inventory inventory = (Inventory)comparableObject;
                        Item item = (Item)thresholdObject;
                        secondRulesetCondition = inventory.GetConsumableItem(item.ItemName) == null;
                    }
                }
                // If the action that needs to be executed when the comparison is false, invert the condition
                if (builderInfo.RuleSets[1].ComparisonFalse == builderInfo.Action)
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

        private object GetData(object comparisonData, string comparisonString)
        {
            if (comparisonString == "health")
            {
                var data = (ICharacterData)comparisonData;
                return data.WorldService.GetCharacter(data.CharacterId).Health;
            }

            if (comparisonString == "stamina")
            {
                var data = (AgentData)comparisonData;
                return data.Stamina;
            }

            if (comparisonString == "inventory")
            {
                var data = (ICharacterData)comparisonData;
                return data.Inventory;
            }

            if (double.TryParse(comparisonString, out _))
            {
                return double.Parse(comparisonString);
            }

            return comparisonData;
        }

        private bool IsInstanceOf(object comparisonData, string comparisonString)
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

        public List<KeyValuePair<string, CharacterState>> GetActionWithStateList()
        {
            return new List<KeyValuePair<string, CharacterState>>
            {
                //new("engage", new EngageState(_characterData, _stateMachine)),
                //new("collect", new CollectState(_characterData, _stateMachine)),
                new("follow", new FollowCreatureState(_characterData, _stateMachine)),
                new("flee", new FleeFromCreatureState(_characterData, _stateMachine)),
                new("attack", new AttackState(_characterData, _stateMachine)),
                new("idle", new IdleState(_characterData, _stateMachine)),
                new("inventory", new InventoryState(_characterData, _stateMachine)),
                new("wander", new WanderState(_characterData, _stateMachine))
            };
        }
    }
}
