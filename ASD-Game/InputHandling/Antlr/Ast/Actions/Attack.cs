using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast.Actions
{
    public class Attack : Command, IEquatable<Attack>
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
            return Equals(obj as Attack);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Attack other)
        {
            if (other == null)
            {
                return false;
            }

            return _direction.Equals(other._direction);
        }
        public override int GetHashCode()
        {
            return _direction.GetHashCode();
        }
    }
}