using Agent.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Creature.Creature.StateMachine.CustomRuleSet
{
    public class RuleSetFactory
    {
        public static List<RuleSet> GetRuleSetListFromSettingsList(List<ValueTuple<string, string>> rulesetSettingsList)
        {
            List<RuleSet> rulesetList = new();
            RuleSet ruleset = new();

            foreach (var currentSetting in rulesetSettingsList)
            {
                if (currentSetting.Item1.EndsWith("aggressiveness") ||
                    currentSetting.Item1.EndsWith("explore") ||
                    currentSetting.Item1.EndsWith("combat"))
                {
                    if (!string.IsNullOrEmpty(ruleset.Setting))
                    {
                        rulesetList.Add(ruleset);
                    }

                    ruleset = new();
                    ruleset.Setting = currentSetting.Item1;
                    ruleset.ComparisonTrue = currentSetting.Item2;
                }
                else if (currentSetting.Item1.EndsWith("comparable"))
                {
                    if (!string.IsNullOrEmpty(ruleset.Setting))
                    {
                        rulesetList.Add(ruleset);
                    }

                    ruleset = new();
                    ruleset.Setting = currentSetting.Item1.Split("_")[0];
                    ruleset.Action = currentSetting.Item1.Split("_")[1];
                    ruleset.Comparable = currentSetting.Item2;
                }
                else if (currentSetting.Item1.EndsWith("threshold"))
                {
                    ruleset.Threshold = currentSetting.Item2;
                }
                else if (currentSetting.Item1.EndsWith("comparison"))
                {
                    ruleset.Comparison = currentSetting.Item2;
                }
                else if (currentSetting.Item1.EndsWith("true"))
                {
                    ruleset.ComparisonTrue = currentSetting.Item2;
                }
                else if (currentSetting.Item1.EndsWith("false"))
                {
                    ruleset.ComparisonFalse = currentSetting.Item2;
                }
            }

            return rulesetList;
        }
    }
}
