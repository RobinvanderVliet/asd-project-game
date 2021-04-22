using System;

namespace Agent.antlr.ast.implementation
{
    /*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: [to be determined].

    This file is created by team: 1.
     
    Goal of this file: [making_the_system_work].
     
    */
    public class AST
    {
        public Configuration root;

        public AST( )
        {
            this.root = new Configuration();
        }
        public AST(Configuration root)
        {
            this.root = root;
        }

        public void SetRoot(Configuration configuration)
        {
            root = configuration;
        }
    }
}
