using ASD_Game.Agent.Antlr.Ast;
using ASD_Game.Agent.Antlr.Ast.Comparables;
using ASD_Game.Agent.Antlr.Ast.Comparables.Subjects;
using System;
using System.Collections.Generic;
using Action = ASD_Game.Agent.Antlr.Ast.Action;

namespace ASD_Game.Agent.Antlr.Checker
{
    public class Checking
    {
        private readonly List<string> _actions = new()
        {
            "engage", "collect"
        };
        private readonly List<string> _actionReferences = new()
        {
            "walk", "attack", "grab", "drop", "flee", "use", "replace"
        };    

        private readonly List<string> _settings = new()
        {
            "explore", "combat"
        };

        private readonly List<string> _ruleValues = new ()
        {
            "random", "circle", "line", "none", "defensive", "offensive"
        };
        
        private  readonly List<string> _weapons = new ()
        {
            "knife", "baseball-bat", "katana", "glock", "p90", "ak-47"
        };
        private readonly List<string> _consumables = new ()
        {
            "bandage", "morphine", "medkit", "big-mac", "monster-energy", "suspicious-white-powder", "lodine-tablets"
        };            
        private readonly List<string> _armor = new ()
        {
            "jacket", "flak-vest", "tactical-vest", "bandana", "hard-hat", "military-helmet", "gas-mask","hazmat-suit"
        };

        private readonly List<string> _bitcoinItems = new()
        {
            "scrap-electronics", "gpu-upgrade", "usb-stick", "bitcoin-wallet"
        };

        private readonly List<string> _tiles = new()
        {
            "street", "grass", "dirt", "water", "office-space", "airplane", 
            "house","gas", "spikes","fire", "health-boost", "stamina-boost",     
            "bitcoin-mining-farm", "chest", "door", "wall"
        };
        
        public virtual void Check(Node node)
        {
            switch (node)
            {
                case ActionReference actionReferenceNode:
                    CheckActionReference(actionReferenceNode);
                    break;
                case Rule ruleNode:
                    CheckRule(ruleNode);
                    break;
                case Condition conditionNode:
                    CheckCondition(conditionNode);
                    break;
                case When whenNode:
                    CheckWhen(whenNode);
                    break;
                case Action actionNode:
                    CheckAction(actionNode);
                    break;
                case Item itemNode:
                    CheckItem(itemNode);
                    break;
                case Tile tileNode:
                    CheckTile(tileNode);
                    break;
            }
            
            foreach (var child in node.GetChildren())
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
            foreach (Condition condition in node.Conditions)
            {
                if (condition.OtherwiseClause != null)
                {
                    if (condition.OtherwiseClause.Action.Name.Equals(node.Name))
                    {
                        node.SetError("No recursion allowed!");
                    }
                }
                if (condition.WhenClause.Then.Name.Equals(node.Name))
                {
                    node.SetError("No recursion allowed!");
                }
            }
        }

        private void CheckActionReference(ActionReference node)
        {
            if (node.Name.Equals("use"))
            {
                if (node.Item == null)
                {
                    node.SetError("Use must be followed by an item!");
                }
            }
            if (!IsActionReference(node.Name))
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
            if (node.Comparison != null)
            {
                CheckComparison(node, node.Comparison, node.ComparableL, node.ComparableR);
            }
            
        }

        private void CheckComparison(When when, Comparison node, Comparable comparableL, Comparable comparableR)
        {
            switch (node.ComparisonType)
            {
                case "contains":
                case "does not contain":
                    CheckContains(when, node, comparableL, comparableR);
                    break;
                case "greater than":
                case "less than":
                case "equals":
                    CheckValueComparison(when, node, comparableL, comparableR);
                    break;
                case "nearby":
                case "finds":
                    CheckSubjectComparison(when, node, comparableL, comparableR);
                    break;
                default:
                    when.SetError("'" + node.ComparisonType + "' does not exist");
                    break;
            }
        }
        
        private void CheckContains(When when, Comparison node,Comparable comparableL, Comparable comparableR)
        {
            if (comparableL is not Inventory)
            {
                when.SetError("Left side of the comparison '" + node.ComparisonType + "' must be inventory!");
            }

            if (comparableR is not Item)
            {
                when.SetError("Right side of the comparison '" + node.ComparisonType + "' must be an item!");
            }
        }

        private void CheckValueComparison(When when, Comparison node,Comparable comparableL, Comparable comparableR)
        {
            if (comparableL is not Stat)
            {
                when.SetError("Left side of the comparison '" + node.ComparisonType + "' must be a stat!");
            }
            if (comparableR is not Int)
            {
                when.SetError("Right side of the comparison '" + node.ComparisonType + "' must be an int value!");
            }
        }

        private void CheckSubjectComparison(When when, Comparison node,Comparable comparableL, Comparable comparableR)
        {
            if (comparableL is not AgentSubject)
            {
                when.SetError("Left side of the comparison '" + node.ComparisonType + "' must be agent!");
            }
            if (comparableR is AgentSubject or Inventory)
            {
                when.SetError("Right side of the comparison '" + node.ComparisonType + "' cant be agent or inventory");
            } 
            else if (comparableR is not Subject)
            {
                when.SetError("Right side of the comparison '" + node.ComparisonType + "' must be a subject!");
            }
        }

        private void CheckTile(Tile node)
        {
            if (!IsTile(node.Name))
            {
                node.SetError("'" + node.Name + "' is not a valid tile!");
            }
        }
        
        private void CheckItem(Item node)
        {
            if (!IsItem(node.Name))
            {
                node.SetError("'" + node.Name + "' is not a valid item!");
            }
        }

        private bool IsSetting(string name)
        {
            return _settings.Contains(name);
        }

        private bool IsRuleValue(string value)
        {
            return _ruleValues.Contains(value);
        }

        private bool IsActionReference(string name)
        {
            return _actionReferences.Contains(name) || IsAction(name);
        }
        
        private bool IsAction(string name)
        {
            return _actions.Contains(name);
        }

        private bool IsItem(string name)
        {
            return _armor.Contains(name) || _bitcoinItems.Contains(name) || _consumables.Contains(name) ||
                   _weapons.Contains(name);
        }

        private bool IsTile(string name)
        {
            return _tiles.Contains(name);
        }
    }
}

