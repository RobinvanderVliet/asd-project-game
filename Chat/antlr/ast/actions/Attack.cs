/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Attack action class (extends command).
     
*/

using System;
using System.Collections;

namespace Chat.antlr.ast.actions
{
    public class Attack : Command, IEquatable<Attack>
    {
        public Direction direction;

        public override ArrayList getChildren()
        {
            var children = new ArrayList();
            children.Add(direction);
            return children;
        }

        public override ASTNode addChild(ASTNode child)
        {
            if (child is Direction)
            {
                direction = (Direction) child;
            }

            return this;
        }

        public override ASTNode removeChild(ASTNode child)
        {
            if (child is Direction && child.Equals(direction))
            {
                direction = null;
            }

            return this;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Attack);
        }

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