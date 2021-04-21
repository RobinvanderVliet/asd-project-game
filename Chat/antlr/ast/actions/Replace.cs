using System;
using System.Diagnostics.CodeAnalysis;

namespace Chat.antlr.ast.actions
{
    public class Replace : Command, IEquatable<Replace>
    {
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Replace);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Replace other)
        {
            return true;
        }
    }
}