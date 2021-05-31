using System.Collections.Generic;
using System.Linq;

namespace Creature.Creature.StateMachine
{
    public class RuleSetFactory
    {
        public static List<RuleSet> GetRuleSetListFromDictionaryList (List<Dictionary<string, string>> rulesetDictionaryList)
        {
            List<RuleSet> rulesetList = new();

            foreach (var block in rulesetDictionaryList)
            {
                RuleSet ruleset = new();

                ruleset.Setting = block.First().Key.Split("_")[0];
                ruleset.Action = block.First().Key.Split("_")[1];

                foreach (KeyValuePair<string, string> entry in block)
                {
                    if (entry.Key.EndsWith("comparable"))
                    {
                        ruleset.Comparable = entry.Value;
                    }
                    else if (entry.Key.EndsWith("threshold"))
                    {
                        ruleset.Threshold = entry.Value;
                    }
                    else if (entry.Key.EndsWith("comparison"))
                    {
                        ruleset.Comparison = entry.Value;
                    }
                    else if (entry.Key.EndsWith("true"))
                    {
                        ruleset.ComparisonTrue = entry.Value;
                    }
                    else if (entry.Key.EndsWith("false"))
                    {
                        ruleset.ComparisonFalse = entry.Value;
                    }
                }

                rulesetList.Add(ruleset);
            }

            return rulesetList;
        }

        public static List<RuleSet> GetRuleSetListFromSettingsList(List<Setting> rulesetSettingsList)
        {
            List<RuleSet> rulesetList = new();

            RuleSet ruleset = new();

            foreach (var currentSetting in rulesetSettingsList)
            {
                if (currentSetting.Property.EndsWith("comparable"))
                {
                    ruleset = new();
                    ruleset.Setting = currentSetting.Property.Split("_")[0];
                    ruleset.Action = currentSetting.Property.Split("_")[1];
                    ruleset.Comparable = currentSetting.Value;
                }
                else if (currentSetting.Property.EndsWith("threshold"))
                {
                    ruleset.Threshold = currentSetting.Value;
                }
                else if (currentSetting.Property.EndsWith("comparison"))
                {
                    ruleset.Comparison = currentSetting.Value;
                }
                else if (currentSetting.Property.EndsWith("true"))
                {
                    ruleset.ComparisonTrue = currentSetting.Value;
                }
                else if (currentSetting.Property.EndsWith("false"))
                {
                    ruleset.ComparisonFalse = currentSetting.Value;
                }

                rulesetList.Add(ruleset);
            }

            return rulesetList;
        }
    }
}
