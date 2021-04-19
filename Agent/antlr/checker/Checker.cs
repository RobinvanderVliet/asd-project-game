using System;
using System.Collections;
using System.Collections.Generic;
using Agent.antlr.ast;
using Agent.antlr.ast.comparables;
using Agent.antlr.grammar;

namespace Agent.antlr.checker
{
    public class Checker
    {
        private ArrayList symboltable;

        public Checker(AST ast)
        {
            foreach (Node node in ast.getChildren())
            {
                symboltable.Add(node);
            }

            CheckStatCombination(symboltable);
        }

        private void CheckStatCombination(ArrayList nodes)
        {
            foreach (Node node in nodes)
            {
                if (node.GetChildren().Count > 0)
                {
                    CheckStatCombination(node.GetChildren());
                }
                else if (node.GetNodeType() == "When")
                {
                    var comparable = (Comparable)node.GetChildren()[0];
                    var stat = (Comparable)node.GetChildren()[2]; // stat;

                    //Itemstat ?
                    if (comparable.Equals("Item"))
                    {
                        CheckItemAndAllowedStat("Weapon", "Power", comparable);
                        CheckItemAndAllowedStat("Potion", "Health", comparable);
                    }
                    else if (stat.Equals("stat"))
                    {
                        
                        
                    }
                    
                }
            }
        }

        private void CheckItemAndAllowedStat(String item, String allowedStat, Comparable comparable)
        {
            if((comparable.GetChildren()[0] == item && comparable.GetChildren()[1] != allowedStat))
            {
                var errorMessage = item + " can only have" + allowedStat + "as stat."; 
            }

        }
    }
}