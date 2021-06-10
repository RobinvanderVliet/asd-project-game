using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast.Actions
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
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}