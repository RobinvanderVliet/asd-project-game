using System;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.Antlr.Ast.Actions
{
    public class Slash : Command, IEquatable<Slash>
    {
        private Direction _direction;
        public Direction Direction { get => _direction; private set => _direction = value; }

        public override ASTNode AddChild(ASTNode child)
        {
            if (child is Direction)
            {
                _direction = (Direction)child;
            }

            return this;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Slash);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Slash other)
        {
            if (other == null)
            {
                return false;
            }

            return _direction.Equals(other._direction);
        }
    }
}