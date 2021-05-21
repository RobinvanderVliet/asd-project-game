using System;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.Antlr.Ast.Actions
{
    public class Pause : Command, IEquatable<Pause>
    {
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Pause);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Pause other)
        {
            return true;
        }
    }
}