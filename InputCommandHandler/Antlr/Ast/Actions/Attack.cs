using System;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.antlr.ast.actions
{
    public class Attack : Command, IEquatable<Attack>
    {
        public Direction direction;

        public override ASTNode AddChild(ASTNode child)
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