using System.Collections.Generic;

namespace Agent.antlr.ast
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].

    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
    public class Configuration : Node
    {
        public Configuration(){}
        
        public Configuration(List<Node> body)
        {
            this.body = body;
        }

        public override string GetNodeType()
        {
            return "Configuration";
        }
    }
}