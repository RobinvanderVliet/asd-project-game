using System;
using System.Diagnostics.CodeAnalysis;

namespace InputCommandHandler.antlr.ast.actions
{
    public class Resume : Command, IEquatable<Resume>
    {
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Resume);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Resume other)
        {
            return true;
        }
    }
}