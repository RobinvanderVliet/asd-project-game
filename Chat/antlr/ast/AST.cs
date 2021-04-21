/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: AST class.
     
*/

using System;
using System.Diagnostics.CodeAnalysis;

namespace Chat.antlr.ast
{
    public class AST : IEquatable<AST>
    {
        public Input root;

        public AST()
        {
            root = new Input();
        }

        public AST(Input input)
        {
            root = input;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as AST);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(AST other)
        {
            if (other == null)
                return false;

            return root.Equals(other.root);
        }
    }
}