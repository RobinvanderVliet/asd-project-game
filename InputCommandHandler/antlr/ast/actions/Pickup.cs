using System;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.antlr.ast.actions
{
    public class Pickup : Command, IEquatable<Pickup>
    {
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Pickup);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Pickup other)
        {
            return true;
        }
    }
}