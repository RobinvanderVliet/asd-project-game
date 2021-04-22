using System;
using System.Diagnostics.CodeAnalysis;

namespace Chat.antlr.ast.actions
{
    public class Pause : Command, IEquatable<Pause>
    {
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Pause);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Pause other)
        {
            return true;
        }
    }
}