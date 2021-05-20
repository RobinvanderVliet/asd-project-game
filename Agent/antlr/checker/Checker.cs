using System;
using System.Collections.Generic;
using System.Linq;
using Agent.antlr.ast;
using Agent.antlr.ast.comparables;


namespace Agent.antlr.checker
{
    public class Checker
    {
        private List<Node> _symboltable;

        public Checker(AST ast)
        {
            foreach (Node node in ast.root.GetChildren())
            {
                _symboltable.Add(node);
            }
            //Entry of checkStatCombination in Pipeline
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

        public Boolean CheckItemAndAllowedStat(Item comparable)
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

        public virtual void Check(AST ast)
        {
            throw new NotImplementedException();
        }
    }
}

