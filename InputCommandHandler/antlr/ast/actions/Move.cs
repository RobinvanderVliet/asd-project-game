using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.antlr.ast.actions
{
    public class Move : Command, IEquatable<Move>
    {
        public Direction direction;
        public Step steps = new Step();
        
        public ArrayList GetChildren()
        {
            var children = new ArrayList();
            children.Add(direction);
            children.Add(steps);
            return children;
        }
        
        public override ASTNode AddChild(ASTNode child)
        {
            if (child is Direction)
            {
                direction = (Direction) child;
            }
            else if (child is Step)
            {
                steps = (Step) child;
            }

            return this;
        }
        
        public ASTNode RemoveChild(ASTNode child)
        {
            if (child is Direction && child == direction)
            {
                direction = null;
            }
            else if (child is Step && child == steps)
            {
                steps = null;
            }

            return this;
        }
        
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Move);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Move other)
        {
            if (other == null)
                return false;

            return direction.Equals(other.direction) && steps.Equals(other.steps);
        }
    }
}