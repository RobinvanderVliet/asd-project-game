using Agent.antlr.ast.interfaces.comparables;

namespace Agent.antlr.ast.implementation.comparables
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].

    This file is created by team: 1].
     
    Goal of this file: [making_the_system_work].
         
    */

    
    public class Stat : Comparable, IStat
    {
        public string Name { get; set; }
        
        public Stat(string name)
        {
            Name = name;
        }

        public new string GetNodeType()
        {
            return "Stat";
        }
        
    }
}