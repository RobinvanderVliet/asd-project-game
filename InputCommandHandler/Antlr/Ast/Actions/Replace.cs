using System;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.Antlr.Ast.Actions
{
    public class Replace : Command, IEquatable<Replace>
    {
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Replace);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Replace other)
        {
            return true;
        }
    }
}