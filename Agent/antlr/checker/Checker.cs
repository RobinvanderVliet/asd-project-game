using System;
using System.Collections;
using System.Collections.Generic;
using Agent.antlr.ast;
using Agent.antlr.ast.comparables;
using Agent.antlr.ast.implementation;

namespace Agent.antlr.checker
{
    public class Checker
    {

        // TODO: Je loopt door een list met nodes om een list met nodes te vullen?
        // private List<Node> symboltable;
        // public Checker(AST ast)
        // {
        //     foreach (Node node in ast.root.GetChildren())
        //     {
        //         symboltable.Add(node);
        //     }
        //     // Commented for working Test ( This can be done in the Pipeline )
        //     // CheckStatCombination(symboltable);
        // }

        // TODO: Unit-test
        public void Check(AST ast)
        {
            CheckStatCombination(ast.root.GetChildren());
        }

        private void CheckStatCombination(List<Node> nodes)
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
                    
                    if (comparable.Equals("Item"))
                    {
                        if (!CheckItemAndAllowedStat("Potion", "Health", (Item) comparable))
                        {                              
                            Console.Write(((Item)comparable).Name + " can not have " + ((Stat)comparable.GetChildren()[0]).Name + " as Stat");
                        }
                        else if (!CheckItemAndAllowedStat("Weapon", "Power", (Item) comparable))
                        {
                            Console.Write(((Item)comparable).Name + " can not have " + ((Stat)comparable.GetChildren()[0]).Name + " as Stat");
                        }
                    }
                }
            }
        }
        public Boolean CheckItemAndAllowedStat(String item, String allowedStat, Item comparable)
        {
            bool itemAllowed = false;
            Stat stat = (Stat) comparable.GetChildren()[0];
            
            if (comparable.Name == item && stat.Name == allowedStat)
            {
                itemAllowed = true;
            }

            return itemAllowed;
        }
    }
}

