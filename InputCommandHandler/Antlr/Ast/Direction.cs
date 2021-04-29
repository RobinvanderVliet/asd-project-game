using System;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.antlr.ast
{
    public class Direction : ASTNode, IEquatable<Direction>
    {
        public string value;

        public Direction(string value)
        {
            this.value = value;
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Direction other)
        {
            if (other == null)
                return false;

            return value == other.value;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Direction);
        }
    }
}