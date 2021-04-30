using System;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.Antlr.Ast
{
    public class Step : ASTNode, IEquatable<Step>
    {
        public int value;

        public Step()
        {
            value = 1;
        }

        public Step(int value)
        {
            this.value = value;
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Step other)
        {
            if (other == null)
                return false;

            return value == other.value;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Step);
        }
    }
}