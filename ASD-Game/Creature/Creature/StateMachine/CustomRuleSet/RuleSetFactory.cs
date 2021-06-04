using Agent.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Creature.Creature.StateMachine.CustomRuleSet
{
    public class RuleSetFactory
    {
        public static List<RuleSet> GetRuleSetListFromSettingsList(List<KeyValuePair<string, string>> rulesetSettingsList)
        {
            List<RuleSet> rulesetList = new();
            RuleSet ruleset = new();

            foreach (var currentSetting in rulesetSettingsList)
            {
                if (currentSetting.Key.EndsWith("aggressiveness") ||
                    currentSetting.Key.EndsWith("explore") ||
                    currentSetting.Key.EndsWith("combat"))
                {
                    if (!string.IsNullOrEmpty(ruleset.Setting))
                    {
                        rulesetList.Add(ruleset);
                    }

                    ruleset = new();
                    ruleset.Setting = currentSetting.Key;
                    ruleset.ComparisonTrue = currentSetting.Value;
                }
                else if (currentSetting.Key.EndsWith("comparable"))
                {
                    if (!string.IsNullOrEmpty(ruleset.Setting))
                    {
                        rulesetList.Add(ruleset);
                    }

                    ruleset = new();
                    ruleset.Setting = currentSetting.Key.Split("_")[0];
                    ruleset.Action = currentSetting.Key.Split("_")[1];
                    ruleset.Comparable = currentSetting.Value;
                }
                else if (currentSetting.Key.EndsWith("threshold"))
                {
                    ruleset.Threshold = currentSetting.Value;
                }
                else if (currentSetting.Key.EndsWith("comparison"))
                {
                    ruleset.Comparison = currentSetting.Value;
                }
                else if (currentSetting.Key.EndsWith("true"))
                {
                    ruleset.ComparisonTrue = currentSetting.Value;
                }
                else if (currentSetting.Key.EndsWith("false"))
                {
                    ruleset.ComparisonFalse = currentSetting.Value;
                }
            }

            return rulesetList;
        }
    }
}
