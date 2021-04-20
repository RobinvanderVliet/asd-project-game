using System;
using System.Collections;
using Agent.antlr.ast;
using Agent.antlr.ast.comparables;
using Agent.antlr.checker;
using NUnit.Framework;

namespace Agent.Tests.checker
{
    public class TestCheckerFunctions
    {
        [Test]
        public void CheckItemAndAllowedStat1()
        {
            String testItem = "Weapon";
            String testAllowedStat = "Power";
            
            
            Item item = new Item();
            item.Value = "Weapon";
            Stat stat = new Stat();
            stat.Name = "Power";
            item.AddChild(stat);

            CheckItemAndAllowedStat(testItem, testAllowedStat, item);
            


        }
    }
}



// private void CheckItemAndAllowedStat(String item, String allowedStat, Comparable comparable)
// {
//     if((comparable.GetChildren()[0] == item && comparable.GetChildren()[1] != allowedStat))
//     {
//         var errorMessage = item + " can only have" + allowedStat + "as stat."; 
//     }
//
// }