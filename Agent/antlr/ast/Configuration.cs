using System.Collections.Generic;

namespace Agent.antlr.ast
{

    public class Configuration : Node
    {
        public Configuration(){}

        public override string GetNodeType()
        {
            return "Configuration";
        }
    }
}