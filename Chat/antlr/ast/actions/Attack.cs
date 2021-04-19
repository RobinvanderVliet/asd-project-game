/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Attack action class (extends command).
     
*/

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Chat.antlr.ast.actions
{
    public class Attack : Command, IEquatable<Attack>
    {
        public Direction direction;


        public override ASTNode addChild(ASTNode child)
        {
            if (child is Direction)
            {
                direction = (Direction) child;
            }

            return this;
        }
        

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Attack);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Attack other)
        {
            if (other == null)
            {
                return false;
            }

            return direction.Equals(other.direction);
        }
    }
}