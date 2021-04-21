namespace Agent.antlr.ast.comparables.subjects
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].

    This file is created by team: 1].
     
    Goal of this file: [making_the_system_work].
         
    */

    public class Player : Subject
    {
        public Player(string name) : base(name)
        {
        }


        public string GetNodeType()
        {
            return "Player";
        }
    }
}