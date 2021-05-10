using System;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.Antlr.Ast
{
    public class Step : ASTNode, IEquatable<Step>
    {
        private int _value;
        public int Value { get => _value; private set => _value = value; }
        // public int Value { get; private set; }

        public Step()
        {
            Value = 1;
        }

        public Step(int value)
        {
            Value = value;
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Step other)
        {
            if (other == null)
                return false;

            return Value == other.Value;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Step);
        }
    }
}