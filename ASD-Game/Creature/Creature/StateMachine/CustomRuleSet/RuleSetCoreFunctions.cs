using System.Collections.Generic;
using Creature.Creature.StateMachine.Data;

namespace Creature.Creature.StateMachine.CustomRuleSet
{
    public class RuleSetCoreFunctions
    {
       
        public bool CheckAgainstRuleSet(ICreatureData data, string rule, string value)
        {
            for (int i = 0; i < data.RuleSet.Count; i ++)
            {
                if (data.RuleSet[i].Item1 == rule)
                {
                    if (data.RuleSet[i].Item2 == value)
                    {
                        return true;
                    }
                }    
            } 
            return false;
        }
                
        public List<KeyValuePair<string, string>> RequestRules(ICreatureData data, List<string> requestedRules)
        {
            List<KeyValuePair<string, string>> RequestedRules = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < data.RuleSet.Count; i ++)
            {
                for (int j = 0; j < requestedRules.Count; j++)
                {
                    if (data.RuleSet[i].Item1 == requestedRules[j])
                    {
                        RequestedRules.Add(new KeyValuePair<string, string>(data.RuleSet[i].Item1, data.RuleSet[i].Item2));
                    }
                }
            }
            return RequestedRules;
        }
        
        public bool IsRuleSetSimple(ICreatureData data)
        {
            int hitCounter = 0;
            for (int i = 0; i < data.RuleSet.Count; i ++)
            {
                if (data.RuleSet[i].Item1 == "aggressiveness")
                {
                    hitCounter ++;
                }else if (data.RuleSet[i].Item1 == "explore")
                {
                    hitCounter ++;
                }else if (data.RuleSet[i].Item1 == "combat")
                {
                    hitCounter ++;   
                } 
            }
            if (hitCounter >= 1)
            {
                return true;
            }
            return false;
        }
        
        public string AnalyzeMap()//TODO Build this function and refactor to a proper location
        {
            return null; //Should return what is close can be an Item,Player or Monster
        }
    }
}