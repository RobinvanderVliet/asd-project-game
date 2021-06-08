using System.Collections.Generic;

namespace WorldGeneration.StateMachine.CustomRuleSet
{
    public class RuleSet
    {
        private Dictionary<string, string> _ruleSet { get; }

        public RuleSet(Dictionary<string, string> RuleSet)
        {
            _ruleSet = RuleSet;
        }
    }
}