using System;
using System.Diagnostics.CodeAnalysis;

namespace ASD_Game.InputHandling.Antlr.Ast.Actions
{
    public class Search : Command, IEquatable<Search>
    {
        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            return Equals(obj as Search);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Search other)
        {
            return true;
        }
    }
}