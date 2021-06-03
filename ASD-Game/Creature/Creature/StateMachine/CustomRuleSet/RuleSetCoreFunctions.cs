using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                
        public Dictionary<string, string> RequestRules(ICreatureData data, List<string> requestedRules)
        {
            Dictionary<string, string> RequestedRules = new Dictionary<string, string>();
            for (int i = 0; i < data.RuleSet.Count; i ++)
            {
                for (int j = 0; j < requestedRules.Count; j++)
                {
                    if (data.RuleSet[i].Item1 == requestedRules[j])
                    {
                        if (data.RuleSet[i].Item1 == "combat_default_agent_treshold" 
                            ||data.RuleSet[i].Item1 == "combat_default_agent_comparison" 
                            || data.RuleSet[i].Item1 == "combat_default_agent_comparison_true")
                        {
                            string key = data.RuleSet[i].Item1 + data.RuleSet[i].Item2;
                            RequestedRules.Add(key, data.RuleSet[i].Item2);
                        }
                        else
                        {
                            RequestedRules.Add(data.RuleSet[i].Item1, data.RuleSet[i].Item2);
                        }
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

        public String checkIfAttack(ICreatureData data, string creatureType)
        {
            List<string> requestedRules = new List<string>();
            requestedRules.Add("combat_default_agent_treshold");
            requestedRules.Add("combat_default_agent_comparison");
            requestedRules.Add("combat_default_agent_comparison_true");
            requestedRules.Add("combat_engage_health_treshold");
            requestedRules.Add("combat_engage_health_comparison");
            requestedRules.Add("combat_engage_health_comparison_true");
            requestedRules.Add("combat_engage_health_comparison_false");
            Dictionary<string, string> givenRules = RequestRules(data, requestedRules);

            if (givenRules.ContainsKey("combat_default_agent_treshold" + creatureType)&&
                givenRules.ContainsKey("combat_default_agent_comparison" + creatureType))
            {
                if (givenRules.ContainsKey("combat_default_agent_comparison_true" + creatureType))
                {
                    if (givenRules["combat_default_agent_comparison_true"] == "flee")
                    {
                        return givenRules["combat_default_agent_comparison_true"];
                    }
                }
                
            }

            if (givenRules.ContainsKey("combat_engage_health_treshold")&&
                givenRules.ContainsKey("combat_engage_health_comparison")&&
                givenRules.ContainsKey("combat_engage_health_comparison_true")&&
                givenRules.ContainsKey("combat_engage_health_comparison_false"))
            {
                int heathTreshold = Convert.ToInt32(givenRules["combat_engage_health_treshold"]);
                switch (givenRules["combat_engage_health_comparison"])
                {
                    case "less than":
                        if (heathTreshold < data.Health)
                        {
                            return givenRules["combat_engage_health_comparison_true"];
                        }
                        else
                        {
                            return givenRules["combat_engage_health_comparison_false"];
                        }
                    case "greater than":
                        if (heathTreshold > data.Health)
                        {
                            return givenRules["combat_engage_health_comparison_true"];
                        }
                        else
                        {
                            return givenRules["combat_engage_health_comparison_false"];
                        }
                    case "equals":
                        if (heathTreshold == data.Health)
                        {
                            return givenRules["combat_engage_health_comparison_true"];
                        }
                        else
                        {
                            return givenRules["combat_engage_health_comparison_false"];
                        }
                }
            }

            return "attack";
        }
        
        public string AnalyzeMap()//TODO Build this function and refactor to a proper location
        {
            return null; //Should return what is close can be an Item,Player or Monster
        }
    }
}