
namespace Agent.antlr.ast.comparables
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].

    This file is created by team: 1].
     
    Goal of this file: [making_the_system_work].
         
    */

    
    public class Subject : Comparable
    {
        public string Name { get; set; }
        
        public Subject(string name)
        {
            Name = name;
        }


        public new string GetNodeType()
        {
            return "Subject";
        }
    }
}