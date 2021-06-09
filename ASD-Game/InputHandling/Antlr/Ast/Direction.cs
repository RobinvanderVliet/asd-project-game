using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast
{
    public class Direction : ASTNode, IEquatable<Direction>
    {
        private string _direction;
        public string DirectionValue { get => _direction; private set => _direction = value; }

        public Direction(string direction)
        {
            _direction = direction;
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Direction other)
        {
            if (other == null)
                return false;

            return _direction == other.DirectionValue;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Direction);
        }
        public override int GetHashCode()
        {
            return _direction.GetHashCode();
        }
    }
}