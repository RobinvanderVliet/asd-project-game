using System;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.Antlr.Ast.Actions
{
    public class Exit : Command, IEquatable<Exit>
    {
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Exit);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Exit other)
        {
            return true;
        }
    }
}