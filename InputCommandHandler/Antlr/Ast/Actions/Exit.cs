using System;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.antlr.ast.actions
{
    public class Exit : Command, IEquatable<Exit>
    {
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Exit);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Exit other)
        {
            return true;
        }
    }
}