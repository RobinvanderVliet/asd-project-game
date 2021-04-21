using System;
using System.Collections;
using Agent.antlr.ast.implementation;
using Agent.antlr.ast.implementation.comparables;


namespace Agent.antlr.checker
{
    public class Checker
    {
        private ArrayList symboltable;

        public Checker(AST ast)
        {
            foreach (Node node in ast.root.GetChildren())
            {
                symboltable.Add(node);
            }
            // Commented for working Test ( This can be done in the Pipeline )
            // CheckStatCombination(symboltable);
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

