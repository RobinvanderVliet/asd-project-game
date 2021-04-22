using System;
using System.Diagnostics.CodeAnalysis;

namespace Chat.antlr.ast
{
    public class Step : ASTNode, IEquatable<Step>
    {
        private int value = 1;

        public Step()
        {
        }

        public Step(int value)
        {
            this.value = value;
        }
        
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Step);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Step other)
        {
            if (other == null)
                return false;

            return value == other.value;
        }
    }
}