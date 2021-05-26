using System;
using System.Collections.Generic;
using System.Linq;
using Agent.Antlr.Ast;
using Agent.Antlr.Ast.Comparables;
using Agent.Antlr.Ast.Comparables.Subjects;
using Action = Agent.Antlr.Ast.Action;

namespace Agent.Antlr.Checker
{
    public class Checking
    { 
    
        
        private List<string> _actions = new List<string>()
        {
            "engage", "collect"
        };
        private List<string> _actionreferences = new List<string>()
        {
            "walk", "attack", "grab", "drop", "flee", "use", "replace"
        };    

        private List<string> _settings = new List<string>()
        {
            "explore", "combat"
        };

        private List<string> _rulevalues = new List<string>()
        {
            "random", "circle", "line", "none", "defensive", "offensive"
        };
        
        private List<Node> _symboltable;

        public Checking(AST ast)
        {
            _actionreferences.AddRange(_actions);
            
            Check(ast.root);
            
            // foreach (Node node in ast.root.GetChildren())
            // {
            //     _symboltable.Add(node);
            // }
            //Entry of checkStatCombination in Pipeline
        }
        public virtual void Check(Node node)
        {
            if (node is ActionReference)
            {
                CheckActionReference((ActionReference) node);
            }
            else if (node is Rule)
            {
                CheckRule((Rule)node);
            }
            else if (node is Condition)
            {
                CheckCondition((Condition) node);
            }
            else if (node is When)
            {
                CheckWhen((When) node);
            }else if (node is Action)
            {
                CheckAction((Action) node);
            }

            foreach (Node child in node.GetChildren())
            {
                Check(child);
            }
        }


        private void CheckAction(Action node)
        {
            if (!IsAction(node.Name))
            {
                node.SetError("'" + node.Name + "' is not a valid/programmable action!");
            }
            
        }

        
        private void CheckActionReference(ActionReference node)
        {
            if (node.Name.Equals("use"))
            {
                if (node.Item == null)
                {
                    node.SetError("use must be followed by an item!");
                }
            }
            if (!IsAction(node.Name))
            {
                node.SetError("'" + node.Name + "' is not a valid action!");
            }
        }

        private void CheckRule(Rule node)
        {
            if (!IsSetting(node.SettingName))
            {
                node.SetError("'" + node.SettingName + "' is not a setting!");
            }

            if (!IsRuleValue(node.Value))
            {
                node.SetError("'" + node.Value + "' is not a valid value!" );
            }
        }

        private void CheckCondition(Condition node)
        {
            Otherwise otherwise = node.OtherwiseClause;
            if (otherwise != null)
            {
                ActionReference otherwiseAction = otherwise.Action;
                ActionReference thenAction = node.WhenClause.Then;
                if (otherwiseAction.Name.Equals(thenAction.Name))
                {
                    node.SetError("Otherwise action cant be the same as then action!");
                }
            }
        }

        private void CheckWhen(When node)
        {
            CheckComparison(node.Comparison, node.ComparableL, node.ComparableR);
            
        }

        private void CheckComparison(Comparison node, Comparable comparableL, Comparable comparableR)
        {
            switch (node.ComparisonType)
            {
                case "contains":
                case "does not contain":
                    CheckContains(node, comparableL);
                    break;
                case "greater than":
                case "less than":
                case "equals":
                    CheckValueComparison(node, comparableL, comparableR);
                    break;
                case "nearby":
                case "finds":
                    CheckSubjectComparison(node, comparableL, comparableR);
                    break;
                default:
                    node.SetError("'" + node.ComparisonType + "' does not exist");
                    break;
            }
        }
        
        private void CheckContains(Comparison node,Comparable comparableL)
        {
            if (!(comparableL is Inventory))
            {
                node.SetError("'" + node.ComparisonType + "' must use inventory! (inventory +" + node.ComparisonType + "...) ");
            }
        }

        private void CheckValueComparison(Comparison node,Comparable comparableL, Comparable comparableR)
        {
            if (!((comparableL is Int || comparableR is Stat) && (comparableL is Stat || comparableR is Stat)))
            {
                node.SetError("'" + node.ComparisonType + "' must be used with int values or Stats only");
            }
        }

        private void CheckSubjectComparison(Comparison node,Comparable comparableL, Comparable comparableR)
        {
            if (!(comparableL is AgentSubject))
            {
                node.SetError("'" + node.ComparisonType + "' must use agent! (agent +" + node.ComparisonType + "...) ");
            }
            if (comparableR is AgentSubject)
            {
                node.SetError("'" + node.ComparisonType + "' cant use agent as the second comparable,");
            }
            else if (comparableR is Inventory)
            {
                node.SetError("'" + node.ComparisonType + "' cant use inventory as the second comparable,");
            }
            else if (!(comparableR is Subject))
            {
                node.SetError("'" + node.ComparisonType + "' must use a subject as the second comparable,");
            }
        }


        private bool IsSetting(string name)
        {
            return _settings.Contains(name);
        }

        private bool IsRuleValue(string value)
        {
            return _rulevalues.Contains(value);
        }

        private bool IsValidActionReference(string name)
        {
            return _actionreferences.Contains(name);
        }
        
        private bool IsAction(string name)
        {
            return _actions.Contains(name);
        }
        
        
        
        
        
        
        public void CheckStatCombination(List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                if (node.GetChildren().Count > 0)
                {
                    CheckStatCombination(node.GetChildren());
                    
                }
                
                if (node is When)
                {
                    var comparable = (Comparable) node.GetChildren().FirstOrDefault();

                    if (comparable is Item)
                    {
                        if (!CheckItemAndAllowedStat((Item) comparable))
                        {
                            comparable.SetError("There is an invalid combination of item and stat");
                        }
                    }
                }
                if (node is Stat)
                {
                    
                }
            }
        }

        public bool CheckItemAndAllowedStat(Item comparable)
        {
            bool itemAllowed = false;
            
            string[][] allowedItemStatsCombinations =
            {
                //              ITEM     STAT
                new[] {"Weapon", "Power"},
                new[] {"Potion", "Health"},
            };

            String itemName = comparable.Name;
            Stat stat = (Stat) comparable.GetChildren()[0];
            String statName = stat.Name;


            foreach (string[] s in allowedItemStatsCombinations)
            {
                if (itemName != s[0] || statName != s[1]) continue;
                itemAllowed = true;
                if (itemAllowed) break;

            }
            return itemAllowed;
        }

      
    }
}

