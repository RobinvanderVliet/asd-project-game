using System;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.Antlr.Ast
{
    public class Direction : ASTNode, IEquatable<Direction>
    {
        private string _value;
        public string Value { get => _value; private set => _value = value; }

        public Direction(string value)
        {
            _value = value;
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Direction other)
        {
            if (other == null)
                return false;

            return _value == other._value;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Direction);
        }
    }
}