using ASD_Game.Creature.Creature.StateMachine.CustomRuleSet;
using System.Collections.Generic;

namespace Creature.Creature.StateMachine.CustomRuleSet
{
    public class RuleSetFactory
    {
        public List<RuleSet> GetRuleSetListFromSettingsList(IEnumerable<KeyValuePair<string, string>> rulesetSettingsList)
        {
            List<RuleSet> rulesetList = new();
            RuleSet ruleset = new();

            foreach (var currentSetting in rulesetSettingsList)
            {
                if (currentSetting.Key.EndsWith("comparable"))
                {
                    if (!string.IsNullOrEmpty(ruleset.Setting))
                    {
                        rulesetList.Add(ruleset);
                    }

                    ruleset = new RuleSet()
                    {
                        Setting = currentSetting.Key.Split("_")[0],
                        Action = currentSetting.Key.Split("_")[1],
                        Comparable = currentSetting.Value
                    };
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

            rulesetList.Add(ruleset);

            return rulesetList;
        }

        public List<KeyValuePair<string, string>> GetSimpleRuleSetListFromSettingsList(List<KeyValuePair<string, string>> rulesetSettingsList)
        {
            List<KeyValuePair<string, string>> rulesetList = new();

            foreach (var currentSetting in rulesetSettingsList)
            {
                if (currentSetting.Key.EndsWith("aggressiveness") ||
                    currentSetting.Key.EndsWith("explore") ||
                    currentSetting.Key.EndsWith("combat"))
                {
                    rulesetList.Add(new KeyValuePair<string, string>(currentSetting.Key, currentSetting.Value));
                }
            }

            return rulesetList;
        }
    }
}
