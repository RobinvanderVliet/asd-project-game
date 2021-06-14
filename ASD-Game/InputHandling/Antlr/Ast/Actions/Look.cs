using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast.Actions
{
    public class Look : Command, IEquatable<Look>
    {
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Look);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Look other)
        {
            return true;
        }
    }
}