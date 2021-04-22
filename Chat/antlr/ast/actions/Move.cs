using System;
using System.Diagnostics.CodeAnalysis;

namespace Chat.antlr.ast.actions
{
    public class Move : Command, IEquatable<Move>
    {
        public Direction direction;
        public Step steps = new Step();
        
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
        

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Move);
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